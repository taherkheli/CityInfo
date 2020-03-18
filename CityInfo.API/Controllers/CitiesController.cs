using AutoMapper;
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
    private readonly IMapper _mapper;

    public CitiesController(ICityInfoRepo cityInfoRepo, IMapper mapper)
    {
      _cityInfoRepo = cityInfoRepo ?? throw new ArgumentNullException(nameof(cityInfoRepo));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public IActionResult GetCities()
    {
      var cityEntities = _cityInfoRepo.GetCities();

      return Ok(_mapper.Map<IEnumerable<CityWithoutPOIsDto>>(cityEntities));
    }

    [HttpGet("{id}")]
    public IActionResult GetCity(int id, bool includePOIs = false)
    {
      var city = _cityInfoRepo.GetCity(id, includePOIs);

      if (city == null)
        return NotFound();

      if (includePOIs)
        return Ok(_mapper.Map<CityDto>(city));      
      else
        return Ok(_mapper.Map<CityWithoutPOIsDto>(city));
    }
  }
}