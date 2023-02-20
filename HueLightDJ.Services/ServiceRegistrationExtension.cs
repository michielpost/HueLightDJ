using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Services
{
  public static class ServiceRegistrationExtension
  {
    public static void AddHueLightDJServices(this IServiceCollection services)
    {
      services.AddSingleton<EffectService>();
      services.AddSingleton<StreamingSetup>();
    }
  }
}
