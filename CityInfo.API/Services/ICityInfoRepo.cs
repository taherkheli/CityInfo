using CityInfo.API.Entities;
using System.Collections.Generic;

namespace CityInfo.API.Services
{
	public interface ICityInfoRepo
	{
		IEnumerable<City> GetCities();

		City GetCity(int id, bool includePOIs);

		POI GetPOIforCity(int id, int poiId);

		IEnumerable<POI> GetPOIsForCity(int id);
	}
}
