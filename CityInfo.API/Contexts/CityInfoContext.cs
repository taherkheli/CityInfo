using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Contexts
{
	public class CityInfoContext : DbContext
	{
		public DbSet<City> Cities { get; set; }
		public DbSet<POI> POIs { get; set; }

		//One way of providing connStr
		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	optionsBuilder.UseSqlServer("connectionString");
		//	base.OnConfiguring(optionsBuilder);
		//}

		//preferred way
		public CityInfoContext(DbContextOptions<CityInfoContext> dbContextOptions)
			:base(dbContextOptions)
		{
			Database.EnsureCreated();   //if DB exists already this will do nothing
		}
	}
}