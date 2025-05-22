using HueApi.Entertainment;
using HueApi.Entertainment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueLightDJ.Services.Models;
using HueLightDJ.Services.Interfaces;
using HueLightDJ.Services.Interfaces.Models;
using HueApi; // Added for LocalHueClient
using Microsoft.Extensions.Logging; // Added for ILogger, though might not be directly available here. Using Console.WriteLine for simplicity if logger DI is complex.

namespace HueLightDJ.Services
{
  public class LightDJStreamingHueClient : StreamingHueClient
  {
    private bool _demoMode;
    private readonly IHubService _hub;
    private string _bridgeIp;
    private string _appKey; // Store appKey for internal client
    private Dictionary<byte, string> _lightIdToNameMap = new Dictionary<byte, string>();


    public LightDJStreamingHueClient(IHubService hub, string ip, string appKey, string clientKey, bool demoMode) : base(ip, appKey, clientKey)
    {
      this._hub = hub;
      _bridgeIp = ip;
      _appKey = appKey; // Store appKey
      _demoMode = demoMode;

      // Initialize light names - this is a fire-and-forget, might need error handling / retry
      _ = InitializeLightNamesAsync();
    }

    private async Task InitializeLightNamesAsync()
    {
      try
      {
        var internalClient = new LocalHueClient(_bridgeIp, _appKey);
        var lights = await internalClient.GetLightsAsync();
        if (lights != null && !lights.HasErrors)
        {
          foreach (var light in lights.Data)
          {
            if (byte.TryParse(light.Id, out byte numericId)) // Hue light IDs are strings "1", "2", etc.
            {
              _lightIdToNameMap[numericId] = light.Name ?? $"Light {numericId}";
            }
          }
        }
        else
        {
            // Log error or handle cases where lights couldn't be fetched
            Console.WriteLine($"Error fetching light names: {lights?.Errors?.FirstOrDefault()?.Description}");
        }
      }
      catch (Exception ex)
      {
        // Log exception
        Console.WriteLine($"Exception initializing light names: {ex.Message}");
      }
    }

    protected override void Send(IEnumerable<IEnumerable<StreamingChannel>> chunks)
    {
      if(!_demoMode)
        base.Send(chunks);

      var flatten = chunks.SelectMany(x => x);

      _hub.SendPreview(flatten.Select(x => new PreviewModel()
      {
        Bridge = _bridgeIp,
        Id = x.Id,
        Name = _lightIdToNameMap.TryGetValue(x.Id, out var name) ? name : $"Light {x.Id}", // Populate Name
        X = x.ChannelLocation.X,
        Y = x.ChannelLocation.Y,
        Hex = x.State.RGBColor.ToHex(),
        Bri = x.State.Brightness
      }));
    }
  }
}
