using HueApi.BridgeLocator;
using HueApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLightDJ.Services.Interfaces
{
  public interface IHueSetupService
  {
    Task<List<EntertainmentConfiguration>> GetEntertainmentGroupsAsync(string ip, string key);
    Task<IEnumerable<LocatedBridge>> LocateBridgesAsync();
  }
}
