using HueLightDJ.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Maui.Services
{
  public class HubService : IHubService
  {
    public Task SendAsync(string method, object arg1)
    {
      return Task.CompletedTask;
    }

    public Task SendAsync(string method, object arg1, object arg2)
    {
      return Task.CompletedTask;
    }
  }
}
