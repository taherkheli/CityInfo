using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Controllers
{
  [ApiController]
  [Route("api/cities/")]
  public class CitiesController : ControllerBase
  {
    private readonly ICityInfoRepo _cityInfoRepo;

    public CitiesController(ICityInfoRepo cityInfoRepo)
    {
      _cityInfoRepo = cityInfoRepo ?? throw new ArgumentNullException(nameof(cityInfoRepo));
    }

    [HttpGet]
    public IActionResult GetCities()
    {
      var cityEntities = _cityInfoRepo.GetCities();
      var result = new List<CityWithoutPOIsDto>() { };

      foreach (var cityEntity in cityEntities)
        result.Add(new CityWithoutPOIsDto()
        {
          Id = cityEntity.Id,
          Name = cityEntity.Name,
          Description = cityEntity.Description
        });     

      return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetCity(int id, bool includePOIs = false)
    {
      var cityEntity = _cityInfoRepo.GetCity(id, includePOIs);

      if (cityEntity == null)
        return NotFound();

      if (includePOIs)
      {
        var resultCity = new CityDto()
        {
          Id = cityEntity.Id,
          Name = cityEntity.Name,
          Description = cityEntity.Description
        };

        foreach (var poi in cityEntity.POIs)
          resultCity.PointsOfInterest.Add(new PointOfInterestDto()
          {
            Id = poi.Id,
            Name = poi.Name,
            Description = poi.Description
          });

        return Ok(resultCity);
      }
      else 
      {
        var resultCityWithoutPOIs = new CityWithoutPOIsDto()
        {
          Id = cityEntity.Id,
          Name = cityEntity.Name,
          Description = cityEntity.Description
        };

        return Ok(resultCityWithoutPOIs);
      }
    }
  }
}