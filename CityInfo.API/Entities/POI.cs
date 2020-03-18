using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities
{
	public class POI
	{
		[Key] //by convention it aint needed but explicit > implicit
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		[MaxLength(200)]
		public string Description { get; set; }

		[ForeignKey("CityId")]    //by convention it ain't needed but explicit > implicit
		public City City { get; set; }

		public int CityId { get; set; }
	}
}