using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueLightDJ.Web.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HueLightDJ.Services;
using HueLightDJ.Web.Services;
using HueLightDJ.Services.Models;

namespace HueLightDJ.Web
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public static IConfiguration Configuration { get; set; }

    public static IServiceProvider ServiceProvider { get; set; }


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<CookiePolicyOptions>(options =>
      {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
      });


      services.AddControllersWithViews();

      services.AddSignalR();

      services.AddTransient<IHubService, HubService>();
      services.Configure<List<GroupConfiguration>>(Configuration.GetSection("HueSetup"));
      services.AddHueLightDJServices();

      services.AddCors(options => options.AddPolicy("CorsPolicy",
      builder =>
      {
        builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin();
      }));


      //services.AddSwaggerGen(c =>
      //{
      //  c.SwaggerDoc("v1", new Info { Title = "HueLightDJ API", Version = "v1" });
      //});

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      ServiceProvider = app.ApplicationServices;

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseCookiePolicy();

      app.UseCors("CorsPolicy");

      //app.UseSwagger();
      //app.UseSwaggerUI(c =>
      //{
      //  c.SwaggerEndpoint("/swagger/v1/swagger.json", "HueLightDJ API V1");
      //});

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapHub<StatusHub>("/statushub");
        endpoints.MapHub<PreviewHub>("/previewhub");

        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
