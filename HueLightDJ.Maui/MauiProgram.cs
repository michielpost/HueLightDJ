using HueLightDJ.Maui.Data;
using HueLightDJ.Maui.Services;
using HueLightDJ.Services;
using HueLightDJ.Services.Models;
using Microsoft.Extensions.Logging;

namespace HueLightDJ.Maui
{
  public static class MauiProgram
  {
    public static MauiApp CreateMauiApp()
    {
      var builder = MauiApp.CreateBuilder();
      builder
          .UseMauiApp<App>()
          .ConfigureFonts(fonts =>
          {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
          });

      builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

      builder.Services.AddTransient<IHubService, HubService>();
      builder.Services.Configure<List<GroupConfiguration>>(GetConfig);
      builder.Services.AddHueLightDJServices();

      return builder.Build();
    }

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
