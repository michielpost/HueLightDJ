using Microsoft.AspNetCore.SignalR;
using HueApi.Entertainment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.BlazorWeb.Server.Hubs
{
  public class PreviewHub : Hub
  {

    public PreviewHub()
    { 
    }

    public async Task Connect()
    {
      await Clients.All.SendAsync("StatusMsg", $"Connected!");
    }

  }
}
