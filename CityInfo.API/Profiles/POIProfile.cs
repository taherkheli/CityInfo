﻿using AutoMapper;

namespace CityInfo.API.Profiles
{
	public class POIProfile : Profile
	{
		public POIProfile()
		{
			CreateMap<Entities.POI, Models.PointOfInterestDto>();
			CreateMap<Models.PointOfInterestForCreationDto, Entities.POI>();
			CreateMap<Models.PointOfInterestForUpdateDto, Entities.POI>()
				.ReverseMap();
		}
	}
}