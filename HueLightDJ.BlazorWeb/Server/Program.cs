using Microsoft.AspNetCore.ResponseCompression;
using HueLightDJ.Services;
using HueLightDJ.Services.Interfaces.Models;
using HueLightDJ.Services.Interfaces;
using HueLightDJ.BlazorWeb.Server.Services;
using ProtoBuf.Grpc.Server;
using HueLightDJ.BlazorWeb.Server.Hubs;

namespace HueLightDJ.BlazorWeb
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.

      builder.Services.AddControllersWithViews();
      builder.Services.AddRazorPages();
      builder.Services.AddSignalR();

      builder.Services.AddGrpc();
      builder.Services.AddResponseCompression(opts =>
      {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            new[] { "application/octet-stream" });
      });
      builder.Services.AddCodeFirstGrpc();

      builder.Services.AddSingleton<IHubService, HubService>();
      builder.Services.Configure<List<GroupConfiguration>>(GetConfig);
      builder.Services.AddHueLightDJServices();


      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseWebAssemblyDebugging();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();

      app.UseBlazorFrameworkFiles();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseGrpcWeb();
      app.MapGrpcService<HueSetupService>().EnableGrpcWeb();
      app.MapGrpcService<LightDJService>().EnableGrpcWeb();

      app.MapHub<PreviewHub>("/previewhub");

      app.MapRazorPages();
      app.MapControllers();
      app.MapFallbackToFile("index.html");

      app.Run();
    }

    //TODO: Remove
    private static void GetConfig(List<GroupConfiguration> obj)
    {
      obj.Add(new GroupConfiguration
      {
        Name = "Home",
        Connections = new List<ConnectionConfiguration>
         {
           new ConnectionConfiguration
           {
              Ip = "192.168.0.4",
              Key = "im5PBqU--4CJq2N2t8xMVNvZ2qxOtgzLcfVTkwzP",
              EntertainmentKey =  "32C1FEB5439F313891C44369FF71388C",
              GroupId  =  Guid.Parse("1b9e4f91-d0de-45a6-b525-1e3a1c140399")
           }
         }

      });
    }
  }
}
