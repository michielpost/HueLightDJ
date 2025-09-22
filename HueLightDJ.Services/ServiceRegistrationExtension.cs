using Microsoft.Extensions.DependencyInjection;

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
