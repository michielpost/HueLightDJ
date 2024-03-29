using HueApi;
using HueLightDJ.Services.Interfaces;
using HueLightDJ.Services.Interfaces.Models;
using HueLightDJ.Services.Interfaces.Models.Requests;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Services
{
  public class HueSetupService : IHueSetupService
  {
    public async Task<IEnumerable<SimpleEntertainmentGroup>> GetEntertainmentGroupsAsync(HueSetupRequest request, CallContext context = default)
    {
      var hueClient = new LocalHueApi(request.Ip, request.Key);
      var entConfigsResult = await hueClient.GetEntertainmentConfigurationsAsync();

      return entConfigsResult.Data.Select(x => new SimpleEntertainmentGroup()
      {
        Id = x.Id,
        Name = x.Metadata?.Name,
        LightCount = x.Locations.ServiceLocations.Count()
      });
    }

    public async Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(CallContext context = default)
    {
      var bridgeLocator = new HueApi.BridgeLocator.HttpBridgeLocator();
      IEnumerable<LocatedBridge> ips = new List<LocatedBridge>();

      try
      {
        var result = await bridgeLocator.LocateBridgesAsync(TimeSpan.FromSeconds(2));
        ips = result.Select(x => new LocatedBridge() { IpAddress = x.IpAddress, BridgeId = x.BridgeId, Port = x.Port });
      }
      catch { }

      return ips;
    }

    public async Task<Interfaces.Models.RegisterEntertainmentResult?> RegisterAsync(HueSetupRequest request, CallContext context = default)
    {
      request.Ip = request.Ip.Trim();

      if (request.Ip.Contains(":") || request.Ip.Contains("/"))
        throw new Exception($"Not a valid ip: {request.Ip}");

      var result = await LocalHueApi.RegisterAsync(request.Ip, "HueLightDJ", "Web", generateClientKey: true);
      if (result == null)
        return null;

      return new Interfaces.Models.RegisterEntertainmentResult()
      {
        Ip = request.Ip,
        StreamingClientKey = result.StreamingClientKey,
        Username = result.Username
      };
    }
  }
}
