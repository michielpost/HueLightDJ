using HueLightDJ.Services;
using HueLightDJ.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Services
{
  public class HubService : IHubService
  {
    private IHubContext<PreviewHub> _hub;

    public HubService(IHubContext<PreviewHub> hub)
    {
      _hub = hub;
    }

    public Task SendAsync(string method, object? arg1)
    {
      return _hub.Clients.All.SendAsync(method, arg1);
    }

    public Task SendAsync(string method, object? arg1, object? arg2)
    {
      return _hub.Clients.All.SendAsync(method, arg1, arg2);
    }
  }
}
