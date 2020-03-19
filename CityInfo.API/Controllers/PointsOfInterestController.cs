using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Controllers
{
  [ApiController]
  [Route("api/cities/{id}/pointsofinterest")]
  public class PointsOfInterestController : ControllerBase
  {
    private readonly ILogger<PointsOfInterestController> _logger;
    private readonly IMailService _mailService;
    private readonly ICityInfoRepo _cityInfoRepo;
    private IMapper _mapper;

    public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityInfoRepo cityInfoRepo, IMapper mapper)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
      _cityInfoRepo = cityInfoRepo ?? throw new ArgumentNullException(nameof(cityInfoRepo));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public IActionResult GetPointsOfInterest(int id)
    {
      if (!_cityInfoRepo.CityExists(id))
      {
        _logger.LogInformation($"City with id {id} wasn't found when accessing POI's.");
        return NotFound();      
      }

      var poiEntities = _cityInfoRepo.GetPOIsForCity(id);

      return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(poiEntities));
    }

    [HttpGet("{idpoi}", Name = "GetPointOfInterest")]
    public IActionResult GetPointOfInterest(int id, int idPoi)
    {
      if (!_cityInfoRepo.CityExists(id))
      {
        _logger.LogInformation($"City with id {id} wasn't found when looking for its POI's.");
        return NotFound();
      }

      var poiEntity = _cityInfoRepo.GetPOIforCity(id, idPoi);

      if (poiEntity == null)
        return NotFound();

      return Ok(_mapper.Map<PointOfInterestDto>(poiEntity));
    }

    [HttpPost(Name = "CreatePointOfInterest")]
    public IActionResult CreatePointOfInterest(int id, [FromBody] PointOfInterestForCreationDto pointOfInterest)
    {
      //consumer request could not be de-serialzied into a valid object<PointOfInterestForCreationDto>
      //no need to explicitly write the code below as [ApiController] autmatically takes care of it
      //if (pointOfInterest == null)
      //  return BadRequest();

      //ModelState checks against our input validation rules supplied via data annotation attributes in the model class PointOfInterestForCreationDto
      //no need to explicitly write the code below as [ApiController] autmatically takes care of it
      //if (!ModelState.IsValid)
      //  return BadRequest();

      if (pointOfInterest.Description == pointOfInterest.Name)
        ModelState.AddModelError("Description", "Description cannot be the same as Name");

      if (!ModelState.IsValid)
        return BadRequest(ModelState);     //ModelState has to be provided because Model binding has already occured by now and [ApiController] cant do it for us
      
      //we must check though that the city we want to add POI to does exist
      if (!_cityInfoRepo.CityExists(id))
        return NotFound();

      var poiEntity = _mapper.Map<POI>(pointOfInterest);

      _cityInfoRepo.AddPoiForCity(id, poiEntity);
      _cityInfoRepo.Save();

      //we still want to return a Dto
      var createdPoiToReturn = _mapper.Map<PointOfInterestDto>(poiEntity);

      return CreatedAtRoute("GetPointOfInterest", new { id, idPOI = createdPoiToReturn.Id }, createdPoiToReturn);
    }

    [HttpPut("{idpoi}", Name = "UpdatePointOfInterest")]
    public IActionResult UpdatePointOfInterest(int id, int idPoi, [FromBody] PointOfInterestForUpdateDto pointOfInterest)
    {
      if (pointOfInterest.Description == pointOfInterest.Name)
        ModelState.AddModelError("Description", "Description cannot be the same as Name");

      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      if (!_cityInfoRepo.CityExists(id))
        return NotFound();

      var poiEntity = _cityInfoRepo.GetPOIforCity(id, idPoi);
      if (poiEntity == null)
        return NotFound();

      _mapper.Map(pointOfInterest, poiEntity);   //source is Dto we received, destination is the entity we will persist
      _cityInfoRepo.UpdatePoiForCity(id, poiEntity);
      _cityInfoRepo.Save();

      return NoContent();
    }

    [HttpPatch("{idpoi}", Name = "PartiallyUpdatePointOfInterest")]
    public IActionResult PartiallyUpdatePointOfInterest(int id, int idPoi, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
    {
      if(!_cityInfoRepo.CityExists(id))
        return NotFound();

      var poiEntity = _cityInfoRepo.GetPOIforCity(id, idPoi);
      if (poiEntity == null)
        return NotFound();

      var poiDto = _mapper.Map<PointOfInterestForUpdateDto>(poiEntity);

      //to validate patchDoc we pass on ModelState and afterwards check if it(received JsonPatch doc) is structurally valid
      patchDoc.ApplyTo(poiDto, ModelState);   
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      //here we must ensure that the applied patch did not do any undesired modifications
      if (poiDto.Description == poiDto.Name)
        ModelState.AddModelError("Description", "Description cannot be the same as Name");

      if (!TryValidateModel(poiDto))  
        return BadRequest(ModelState);

      //apply patch to real item
      _mapper.Map(poiDto, poiEntity);   //source is the Dto after application of patch, destination is poi entity we will persist
      _cityInfoRepo.UpdatePoiForCity(id, poiEntity);
      _cityInfoRepo.Save();

      return NoContent();
    }

    [HttpDelete("{idpoi}", Name = "DeletePointOfInterest")]
    public IActionResult DeletePointOfInterest(int id, int idPoi)
    {
      if(!_cityInfoRepo.CityExists(id))
        return NotFound();

      var poiEntity = _cityInfoRepo.GetPOIforCity(id, idPoi);
      if (poiEntity == null)
        return NotFound();

      _cityInfoRepo.DeletePoi(poiEntity);
      _cityInfoRepo.Save();

      _mailService.Send("POI deleted", $"POI {poiEntity.Name} with Id: {poiEntity.Id} was deleted");

      return NoContent();
    }
  }
}