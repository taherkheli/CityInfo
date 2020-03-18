using CityInfo.API.Contexts;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CityInfo.API.Controllers
{
	[ApiController]
	[Route("api/testdatabase")]
	public class DummyController : ControllerBase
	{
		private readonly CityInfoContext _context;

		public DummyController(CityInfoContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}
		
		[HttpGet]
		public IActionResult TestDatabse()
		{
			return Ok();
		}
	}
}
