using Microsoft.AspNetCore.SignalR;
using HueApi.Entertainment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueLightDJ.Services.Models;
using HueLightDJ.Services;
using Microsoft.Extensions.Configuration;
using HueLightDJ.Services.Interfaces.Models;
using Microsoft.Extensions.Options;

namespace HueLightDJ.Web.Hubs
{
  public class PreviewHub : Hub
  {
    private readonly StreamingSetup streamingSetup;
    private readonly EffectService effectService;
    private readonly List<GroupConfiguration> _groupConfigurations;

    public PreviewHub(StreamingSetup streamingSetup, EffectService effectService, IOptions<List<GroupConfiguration>> configOptions)
    {
      this.streamingSetup = streamingSetup;
      this.effectService = effectService;
      this._groupConfigurations = configOptions.Value;
    }

    public Task Connect()
    {
      return Task.CompletedTask;
    }

    public async Task Touch(double x, double y)
    {
      _ = effectService.StartRandomTouchEffect(x, y);
    }

    public async Task GetLocations(string groupName)
    {
      var groupConfig = _groupConfigurations.FirstOrDefault(gc => gc.Name == groupName);
      if (groupConfig == null)
      {
          // Handle error or return empty, e.g., await Clients.Caller.SendAsync("newLocations", new List<MultiBridgeHuePosition>());
          await Clients.Caller.SendAsync("newLocations", new List<MultiBridgeHuePosition>());
          return;
      }
      var locations = await streamingSetup.GetLocationsAsync(groupConfig);
      await Clients.Caller.SendAsync("newLocations", locations);
    }

    public async Task SetLocations(List<MultiBridgeHuePosition> locations)
    {
      await streamingSetup.SetLocations(locations);
    }

    public async Task Locate(MultiBridgeHuePosition light)
    {
      await streamingSetup.AlertLight(light);
    }

  }
}
