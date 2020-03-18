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
			//we do not want this to create a DB when migrations will execute and new up this DbContext. 
			//Migrations will take care of creating the DB 
			//Database.EnsureCreated();   //if DB exists already this will do nothing
		}
	}
}