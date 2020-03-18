using CityInfo.API.Contexts;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CityInfo.API
{
  public class Startup
  {
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
      _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().AddMvcOptions(o => 
      {
        o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
      });
      //.AddJsonOptions( o => 
      //                {
      //                  if (o.SerializerSettings.ContractResolver != null)
      //                  {
      //                    var castedResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
      //                    castedResolver.NamingStrategy = null;
      //                  }
      //                });

      services.AddTransient<IMailService, LocalMailService>();

      var connStr = _configuration["connectionString:cityInfoDBConnectionString"];

      //scoped lifetime by default
      services.AddDbContext<CityInfoContext>(o => {
        o.UseSqlServer(connStr);
      });  
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else 
      {
        app.UseExceptionHandler("/Error");
      }

      app.UseStatusCodePages();
      app.UseMvc();
    }
  }
}
