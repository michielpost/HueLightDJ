using HueLightDJ.BlazorWeb.Client;
using HueLightDJ.Services.Interfaces;
using HueLightDJ.Services.Interfaces.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HueLightDJ.Blazor.Controls;
using HueLightDJ.BlazorWeb.Client.Services;
using HueLightDJ.Services;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Grpc.Net.Client.Web;
using ProtoBuf.Grpc.Client;
using HueLightDJ.Blazor.Controls.Services;

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
      builder.Services.AddSingleton<SelectedConfigState>();
      builder.Services.AddSingleton<ThemeState>(); // Added ThemeState

      builder.Services.AddSingleton<IHubService, SignalRClientHubService>();
      builder.Services.Configure<List<GroupConfiguration>>(GetConfig);

      builder.Services.AddHttpClient("ServerAPI",
              client =>
              {
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
              })
            //.AddHttpMessageHandler<CustomAuthorizationMessageHandler>()
            .AddHttpMessageHandler(() => new GrpcWebHandler(GrpcWebMode.GrpcWeb));

      builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
      .CreateClient("ServerAPI")
      );

      builder.Services.AddSingleton(services =>
      {
        //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
        var httpFactory = services.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpFactory.CreateClient("ServerAPI");
        var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
        var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });
        return channel.CreateGrpcService<IHueSetupService>();
      });

      builder.Services.AddSingleton(services =>
      {
        //var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
        var httpFactory = services.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpFactory.CreateClient("ServerAPI");
        var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
        var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });
        return channel.CreateGrpcService<ILightDJService>();
      });


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
