using HueApi.Entertainment;
using HueApi.Entertainment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueLightDJ.Services.Models;
using HueLightDJ.Services.Interfaces;

namespace HueLightDJ.Services
{
  public class LightDJStreamingHueClient : StreamingHueClient
  {
    private bool _demoMode;
    private readonly IHubService _hub;
    private string _bridgeIp;

    public LightDJStreamingHueClient(IHubService hub, string ip, string appKey, string clientKey, bool demoMode) : base(ip, appKey, clientKey)
    {
      this._hub = hub;
      _bridgeIp = ip;
      _demoMode = demoMode;
     
    }

    protected override void Send(IEnumerable<IEnumerable<StreamingChannel>> chunks)
    {
      if(!_demoMode)
        base.Send(chunks);

      var flatten = chunks.SelectMany(x => x);

      _hub.SendAsync("preview", flatten.Select(x => new PreviewModel()
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
