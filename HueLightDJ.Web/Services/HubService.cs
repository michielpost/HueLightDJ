using HueLightDJ.Services;
using HueLightDJ.Services.Interfaces;
using HueLightDJ.Services.Interfaces.Models;
using HueLightDJ.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
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

#pragma warning disable CS0067 // Event is required by IHubService interface
    public event EventHandler<string?>? LogMsgEvent;
#pragma warning restore CS0067
#pragma warning disable CS0067 // Event is required by IHubService interface
    public event EventHandler? StatusChangedEvent;
#pragma warning restore CS0067
#pragma warning disable CS0067 // Event is required by IHubService interface
    public event EventHandler<IEnumerable<PreviewModel>>? PreviewEvent;
#pragma warning restore CS0067

    public Task SendAsync(string method, params object?[] arg1)
    {
      if (arg1.Length > 1)
      {
        return _hub.Clients.All.SendAsync(method, arg1[0], arg1[1]);

      }
      else
      {
        return _hub.Clients.All.SendAsync(method, arg1[0]);
      }
    }

    public Task SendPreview(IEnumerable<PreviewModel> list)
    {
      return SendAsync("preview", list);
    }

    public Task StatusChanged()
    {
      return Task.CompletedTask;
    }
  }
}
