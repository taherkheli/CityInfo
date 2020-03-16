using System.Collections.Generic;

namespace CityInfo.API.Models
{
  public class CityDto
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int NumberOfPointsOfInterest 
    {
      get 
      {
        return PointsOfInterest.Count;
      } 
    }

    //typically you'd do this instantiation in a ctor but auto property syntax allows you to get away with this instead
    public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();
  }
}
