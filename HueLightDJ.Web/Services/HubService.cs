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

    public HubService()
    {
      var hub = (IHubContext<PreviewHub>?)Startup.ServiceProvider.GetService(typeof(IHubContext<PreviewHub>));
      if (hub == null)
        throw new Exception("Unable to get PreviewHub from ServiceProvider");

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
