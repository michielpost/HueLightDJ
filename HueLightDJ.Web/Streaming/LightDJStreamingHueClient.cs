using HueLightDJ.Web.Hubs;
using HueLightDJ.Web.Models;
using Microsoft.AspNetCore.SignalR;
using Q42.HueApi.Streaming;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Streaming
{
  public class LightDJStreamingHueClient : StreamingHueClient
  {
    private IHubContext<PreviewHub> _hub;
    private bool _demoMode;

    public LightDJStreamingHueClient(string ip, string appKey, string clientKey, bool demoMode) : base(ip, appKey, clientKey)
    {
      _demoMode = demoMode;
      _hub = (IHubContext<PreviewHub>)Startup.ServiceProvider.GetService(typeof(IHubContext<PreviewHub>));
    }

    protected override void Send(IEnumerable<IEnumerable<StreamingLight>> chunks)
    {
      if(!_demoMode)
        base.Send(chunks);

      var flatten = chunks.SelectMany(x => x);

      _hub.Clients.All.SendAsync("preview", flatten.Select(x => new PreviewModel()
      {
        Id = x.Id,
        X = x.LightLocation.X,
        Y = x.LightLocation.Y,
        Hex = x.State.RGBColor.ToHex(),
        Bri = x.State.Brightness
      }));
    }
  }
}
