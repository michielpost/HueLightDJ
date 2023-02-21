using Blazored.LocalStorage;
using HueLightDJ.Blazor.Controls.Services;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Blazor.Controls
{
  public static class ServiceExtensions
  {
    public static void AddHueLightDJBlazorControls(this IServiceCollection services)
    {
      services.AddMudServices();

      services.AddBlazoredLocalStorage();

      services.AddTransient<StorageService>();

    }
  }
}
