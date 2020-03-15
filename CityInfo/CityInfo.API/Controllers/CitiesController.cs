using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
  [ApiController]
  public class CitiesController : ControllerBase
  {
    [HttpGet("api/cities")]
    public JsonResult GetCities()
    {
      return new JsonResult(
        new List<object>()
        {
          new { id = 1, Name = "New York City" },
          new { id = 2, Name = "Antwerp" }
        });
    }
  }
}
