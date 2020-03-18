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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<City>()
				.HasData(
				new City()
				{
					Id = 1,
					Name = "New York City",
					Description = "The one with that big park."
				},
				new City()
				{
					Id = 2,
					Name = "Antwerp",
					Description = "The one with the cathedral that was never really finished."
				},
				new City()
				{
					Id = 3,
					Name = "Paris",
					Description = "The one with that big tower."
				});

			modelBuilder.Entity<POI>()
			.HasData(
				new POI()
				{
					Id = 1,
					CityId = 1,
					Name = "Central Park",
					Description = "The most visited urban park in the United States."
				},
				new POI()
				{
					Id = 2,
					CityId = 1,
					Name = "Empire State Building",
					Description = "A 102-story skyscraper located in Midtown Manhattan."
				},
				new POI()
				{
					Id = 3,
					CityId = 2,
					Name = "Cathedral",
					Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
				},
				new POI()
				{
					Id = 4,
					CityId = 2,
					Name = "Antwerp Central Station",
					Description = "The the finest example of railway architecture in Belgium."
				},
				new POI()
				{
					Id = 5,
					CityId = 3,
					Name = "Eiffel Tower",
					Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel."
				},
				new POI()
				{
					Id = 6,
					CityId = 3,
					Name = "The Louvre",
					Description = "The world's largest museum."
				});

			base.OnModelCreating(modelBuilder);
		}
	}
}