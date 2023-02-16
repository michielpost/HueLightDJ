using Microsoft.AspNetCore.SignalR;
using HueApi.Entertainment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueLightDJ.Services.Models;
using HueLightDJ.Services;
using Microsoft.Extensions.Configuration;

namespace HueLightDJ.Web.Hubs
{
  public class PreviewHub : Hub
  {
    private readonly StreamingSetup streamingSetup;
    private readonly EffectService effectService;

    public PreviewHub(StreamingSetup streamingSetup, EffectService effectService)
    {
      this.streamingSetup = streamingSetup;
      this.effectService = effectService;
    }

    public async Task Connect()
    {

    }

    public async Task Touch(double x, double y)
    {
      effectService.StartRandomTouchEffect(x, y);
    }

    public async Task GetLocations(string groupName)
    {
      var locations = await streamingSetup.GetLocationsAsync(groupName);
      Clients.Caller.SendAsync("newLocations", locations);
    }

    public Task SetLocations(List<MultiBridgeHuePosition> locations)
    {
      return streamingSetup.SetLocations(locations);
    }

    public Task Locate(MultiBridgeHuePosition light)
    {
      return streamingSetup.AlertLight(light);
    }

  }
}
