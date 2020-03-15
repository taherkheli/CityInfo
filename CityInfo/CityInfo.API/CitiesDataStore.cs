﻿using CityInfo.API.Models;
using System.Collections.Generic;

namespace CityInfo.API
{
  public class CitiesDataStore
  {
    public static CitiesDataStore Current { get; } = new CitiesDataStore();

    public List<CityDto> Cities { get; set; }

    public CitiesDataStore()
    {
      // init dummy data
      Cities = new List<CityDto>(){
        new CityDto()
        {
              Id = 1,
              Name = "New York City",
              Description = "The one with that big park.",
              //PointsOfInterest = new List<PointOfInterestDto>()
              //{
              //    new PointOfInterestDto() {
              //        Id = 1,
              //        Name = "Central Park",
              //        Description = "The most visited urban park in the United States." },
              //     new PointOfInterestDto() {
              //        Id = 2,
              //        Name = "Empire State Building",
              //        Description = "A 102-story skyscraper located in Midtown Manhattan." },
              //}
        },
        new CityDto()
        {
            Id = 2,
            Name = "Antwerp",
            Description = "The one with the cathedral that was never really finished."
        },
        new CityDto()
        {
            Id= 3,
            Name = "Paris",
            Description = "The one with that big tower."
        }};
    }
  }
}