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

    [HttpGet("{idPOI}", Name = "GetPointOfInterest")]
    public IActionResult GetPointOfInterest(int id, int idPOI)
    {
      if (!_cityInfoRepo.CityExists(id))
      {
        _logger.LogInformation($"City with id {id} wasn't found when looking for its POI's.");
        return NotFound();
      }

      var poiEntity = _cityInfoRepo.GetPOIforCity(id, idPOI);

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

    [HttpPut("{idPOI}", Name = "UpdatePointOfInterest")]
    public IActionResult UpdatePointOfInterest(int id, int idPOI, [FromBody] PointOfInterestForUpdateDto pointOfInterest)
    {
      if (pointOfInterest.Description == pointOfInterest.Name)
        ModelState.AddModelError("Description", "Description cannot be the same as Name");

      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      if (!_cityInfoRepo.CityExists(id))
        return NotFound();

      var poiEntity = _cityInfoRepo.GetPOIforCity(id, idPOI);
      if (poiEntity == null)
        return NotFound();

      _mapper.Map(pointOfInterest, poiEntity);   //source is Dto we received, destination is the entity we will persist
      _cityInfoRepo.UpdatePoiForCity(id, poiEntity);
      _cityInfoRepo.Save();

      return NoContent();
    }

    [HttpPatch("{idPOI}", Name = "PartiallyUpdatePointOfInterest")]
    public IActionResult PartiallyUpdatePointOfInterest(int id, int idPOI, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
    {
      //creating a separate DTO class fpr Update ops helps here as we do not have to worry about any attempt to update Id

      var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
      if (city == null)
        return NotFound();

      var pointOfInterestFromStore = city.POIs.FirstOrDefault(p => p.Id == idPOI);
      if (pointOfInterestFromStore == null)
        return NotFound();

      var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
      {
        Name = pointOfInterestFromStore.Name,
        Description = pointOfInterestFromStore.Description
      };

      //to validate patchDoc we pass on ModelState and afterwards check if it(received JsonPatch doc) is structurally valid
      patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);   
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      //here we must ensure that the applied patch did not do any undesired modifications
      if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
        ModelState.AddModelError("Description", "Description cannot be the same as Name");

      if (!TryValidateModel(pointOfInterestToPatch))  
        return BadRequest(ModelState);

      //apply patch to real item
      pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
      pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

      return NoContent();
    }

    [HttpDelete("{idPOI}", Name = "DeletePointOfInterest")]
    public IActionResult DeletePointOfInterest(int id, int idPOI)
    {
      var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
      if (city == null)
        return NotFound();

      var pointOfInterestFromStore = city.POIs.FirstOrDefault(p => p.Id == idPOI);
      if (pointOfInterestFromStore == null)
        return NotFound();

      city.POIs.Remove(pointOfInterestFromStore);
      _mailService.Send("POI deleted", $"POI {pointOfInterestFromStore.Name} with Id: {pointOfInterestFromStore.Id} was deleted");

      return NoContent();
    }
  }
}