using HueApi.BridgeLocator;
using HueApi.Models;
using HueApi.Models.Clip;
using HueLightDJ.Services.Interfaces;

namespace HueLightDJ.BlazorWeb.Client.Services
{
  public class HueSetupService : IHueSetupService
  {
    public Task<List<EntertainmentConfiguration>> GetEntertainmentGroupsAsync(string ip, string key)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<LocatedBridge>> LocateBridgesAsync()
    {
      throw new NotImplementedException();
    }

    public Task<RegisterEntertainmentResult?> RegisterAsync(string ip)
    {
      throw new NotImplementedException();
    }
  }
}
