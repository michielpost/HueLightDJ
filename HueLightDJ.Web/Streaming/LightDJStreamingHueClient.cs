using HueLightDJ.Web.Hubs;
using HueLightDJ.Web.Models;
using Microsoft.AspNetCore.SignalR;
using HueApi.Entertainment;
using HueApi.Entertainment.Models;
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
    private string _bridgeIp;

    public LightDJStreamingHueClient(string ip, string appKey, string clientKey, bool demoMode) : base(ip, appKey, clientKey)
    {
      _bridgeIp = ip;
      _demoMode = demoMode;
      var hub = (IHubContext<PreviewHub>?)Startup.ServiceProvider.GetService(typeof(IHubContext<PreviewHub>));
      if (hub == null)
        throw new Exception("Unable to get PreviewHub from ServiceProvider");

      _hub = hub;
    }

    protected override void Send(IEnumerable<IEnumerable<StreamingChannel>> chunks)
    {
      if(!_demoMode)
        base.Send(chunks);

      var flatten = chunks.SelectMany(x => x);

      _hub.Clients.All.SendAsync("preview", flatten.Select(x => new PreviewModel()
      {
        Bridge = _bridgeIp,
        Id = x.Id,
        X = x.ChannelLocation.X,
        Y = x.ChannelLocation.Y,
        Hex = x.State.RGBColor.ToHex(),
        Bri = x.State.Brightness
      }));
    }
  }
}
