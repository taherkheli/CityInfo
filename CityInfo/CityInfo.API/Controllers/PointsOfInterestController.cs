using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

    [HttpGet("{idPOI}")]
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
  }
}