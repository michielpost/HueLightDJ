using HueLightDJ.Web.Models;
using HueLightDJ.Web.Streaming;
using Microsoft.AspNetCore.SignalR;
using HueApi.Entertainment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Hubs
{
  public class PreviewHub : Hub
  {

    public async Task Connect()
    {

    }

    public async Task Touch(double x, double y)
    {
      EffectService.StartRandomTouchEffect(x, y);
    }

    public async Task GetLocations(string groupName)
    {
      var locations = await StreamingSetup.GetLocationsAsync(groupName);
      Clients.Caller.SendAsync("newLocations", locations);
    }

    public Task SetLocations(List<MultiBridgeHuePosition> locations)
    {
      return StreamingSetup.SetLocations(locations);
    }

    public Task Locate(MultiBridgeHuePosition light)
    {
      return StreamingSetup.AlertLight(light);
    }

  }
}
