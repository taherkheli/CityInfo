using CityInfo.API.Contexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CityInfo.API
{
  public class Program
  {
    public static void Main(string[] args)
    {
      //CreateWebHostBuilder(args).Build().Run();
      var host = CreateWebHostBuilder(args).Build();

      using (var scope = host.Services.CreateScope())
      {
        try
        {
          var context = scope.ServiceProvider.GetService<CityInfoContext>();

          //for demo purpose, this will delete the DB every time and migrate to ensure that we start in a clean/known state
          context.Database.EnsureDeleted();
          context.Database.Migrate();
        }
        catch (Exception ex)
        {
        }
      }

      host.Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
  }
}
