using HueLightDJ.BlazorWeb.Client;
using HueLightDJ.Services.Interfaces;
using HueLightDJ.Services.Interfaces.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HueLightDJ.Blazor.Controls;
using HueLightDJ.BlazorWeb.Client.Services;
using HueLightDJ.Services;

namespace HueLightDJ.BlazorWeb.Client
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var builder = WebAssemblyHostBuilder.CreateDefault(args);
      builder.RootComponents.Add<App>("#app");
      builder.RootComponents.Add<HeadOutlet>("head::after");

      builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

      builder.Services.AddHueLightDJBlazorControls();

      builder.Services.AddTransient<ILightDJService, LightDJService>();
      builder.Services.AddTransient<IHueSetupService, HueSetupService>();


      builder.Services.AddSingleton<IHubService, HubService>();
      builder.Services.Configure<List<GroupConfiguration>>(GetConfig);

      await builder.Build().RunAsync();
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
