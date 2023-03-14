using HueLightDJ.Services.Interfaces;

namespace HueLightDJ.BlazorWeb.Client.Services
{
  public class HubService : IHubService
  {
    public event EventHandler<string?>? LogMsgEvent;
    public event EventHandler? StatusChangedEvent;

    public Task SendAsync(string method, params object?[] arg1)
    {
      throw new NotImplementedException();
    }

    public Task StatusChanged()
    {
      throw new NotImplementedException();
    }
  }
}
