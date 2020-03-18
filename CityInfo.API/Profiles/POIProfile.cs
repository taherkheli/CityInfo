using AutoMapper;

namespace CityInfo.API.Profiles
{
	public class POIProfile : Profile
	{
		public POIProfile()
		{
			CreateMap<Entities.POI, Models.PointOfInterestDto>();
		}
	}
}
