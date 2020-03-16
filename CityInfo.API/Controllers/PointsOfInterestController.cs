using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CityInfo.API.Models;

namespace CityInfo.API.Controllers
{
  [ApiController]
  [Route("api/cities/{id}/pointsofinterest")]
  public class PointsOfInterestController : ControllerBase
  {
    [HttpGet]
    public IActionResult GetPointsOfInterest(int id)
    {
      var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);

      if (city == null)
        return NotFound();
      else
        return Ok(city.PointsOfInterest);
    }

    [HttpGet("{idPOI}", Name = "GetPointOfInterest")]
    public IActionResult GetPointOfInterest(int id, int idPOI)
    {
      var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
      if (city == null)
        return NotFound();

      var poi = city.PointsOfInterest.FirstOrDefault(p => p.Id == idPOI);
      if (poi == null)
        return NotFound();

      return Ok(poi);
    }

    [HttpPost]
    public IActionResult CreatePointOfInterest(int id, [FromBody] PointOfInterestForCreationDto pointOfInterest)
    {
      //consumer request could not be de-serialzied into a valid object<PointOfInterestForCreationDto>
      //no need to explicitly write the code below as [ApiController] autmatically takes care of it
      //if (pointOfInterest == null)
      //  return BadRequest();

      //we must check though that the city we want to add POI to does exist
      var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
      if (city == null)
        return NotFound();

      var maxExistingId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

      var finalPointOfInterest = new PointOfInterestDto()
      {
        Id = ++maxExistingId,
        Name = pointOfInterest.Name,
        Description = pointOfInterest.Description
      };

      city.PointsOfInterest.Add(finalPointOfInterest);
      return CreatedAtRoute("GetPointOfInterest", new { id, idPOI = finalPointOfInterest.Id }, finalPointOfInterest);
    }
  }
}