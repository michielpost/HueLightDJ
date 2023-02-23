using HueApi;
using HueApi.BridgeLocator;
using HueApi.Models;
using HueApi.Models.Clip;
using HueLightDJ.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Maui.Services
{
  public class HueSetupService : IHueSetupService
  {
    public async Task<List<EntertainmentConfiguration>> GetEntertainmentGroupsAsync(string ip, string key)
    {
      var hueClient = new LocalHueApi(ip, key);
      var entConfigsResult = await hueClient.GetEntertainmentConfigurationsAsync();

      return entConfigsResult.Data;
    }

    public async Task<IEnumerable<LocatedBridge>> LocateBridgesAsync()
    {
      var bridgeLocator = new HttpBridgeLocator();
      IEnumerable<LocatedBridge> ips = new List<LocatedBridge>();

      try
      {
        ips = await bridgeLocator.LocateBridgesAsync(TimeSpan.FromSeconds(2));
      }
      catch { }

      return ips;
    }

    public Task<RegisterEntertainmentResult?> RegisterAsync(string ip)
    {
      return LocalHueApi.RegisterAsync(ip, "HueLightDJ", "Web", generateClientKey: true);
    }
  }
}
