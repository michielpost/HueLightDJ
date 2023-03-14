using HueLightDJ.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Maui.Services
{
  public class HubService : IHubService
  {
    public event EventHandler<string?>? LogMsgEvent;
    public event EventHandler? StatusChangedEvent;

    public Task StatusChanged()
    {
      StatusChangedEvent?.Invoke(this, EventArgs.Empty);
      return Task.CompletedTask;
    }

    public Task SendAsync(string method, params object?[] arg1)
    {
      if (method == "StatusMsg")
        LogMsgEvent?.Invoke(this, (string?)arg1.FirstOrDefault());

      Console.WriteLine(method);
      return Task.CompletedTask;
    }

  }
}
