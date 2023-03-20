using HueLightDJ.BlazorWeb.Server.Hubs;
using HueLightDJ.Services.Interfaces;
using HueLightDJ.Services.Interfaces.Models;
using Microsoft.AspNetCore.SignalR;

namespace HueLightDJ.BlazorWeb.Server.Services
{
  public class HubService : IHubService
  {
    private IHubContext<PreviewHub> _hub;
    public HubService(IHubContext<PreviewHub> hub)
    {
      _hub = hub;
    }

    public event EventHandler<string?>? LogMsgEvent;
    public event EventHandler? StatusChangedEvent;
    public event EventHandler<IEnumerable<PreviewModel>>? PreviewEvent;

    public async Task SendAsync(string method, params object?[] arg1)
    {
      if (arg1.Length > 1)
      {
        await _hub.Clients.All.SendAsync(method, arg1[0], arg1[1]);

      }
      else
      {
        await _hub.Clients.All.SendAsync(method, arg1[0]);
      }
    }

    public Task SendPreview(IEnumerable<PreviewModel> list)
    {
      return _hub.Clients.All.SendAsync("preview", list.ToList());
      //return SendAsync("preview", list);
    }

    public Task StatusChanged()
    {
      return _hub.Clients.All.SendAsync("StatusChanged");
    }
  }
}
