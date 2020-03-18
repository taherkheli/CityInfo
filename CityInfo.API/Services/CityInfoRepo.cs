using CityInfo.API.Contexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Services
{
	public class CityInfoRepo : ICityInfoRepo
	{
		private readonly CityInfoContext _context;

		public CityInfoRepo(CityInfoContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public IEnumerable<City> GetCities()
		{
			return _context.Cities.OrderBy(c => c.Name).ToList();
		}

		public City GetCity(int id, bool includePOIs)
		{
			if (includePOIs)
				return _context.Cities.Include(c => c.POIs).Where(c => c.Id == id).FirstOrDefault();
			else
				return _context.Cities.Where(c => c.Id == id).FirstOrDefault();
		}

		public POI GetPOIforCity(int id, int poiId)
		{
			return _context.POIs.Where(p => p.CityId == id && p.Id == poiId).FirstOrDefault();
		}

		public IEnumerable<POI> GetPOIsForCity(int id)
		{
			return _context.POIs.Where(p => p.CityId == id).ToList();
		}
	}
}