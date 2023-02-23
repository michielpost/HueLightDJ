using HueLightDJ.Services.Interfaces;

namespace HueLightDJ.BlazorWeb.Client.Services
{
  public class HubService : IHubService
  {
    public event EventHandler<string?>? LogMsgEvent;
    public event EventHandler? StatusChangedEvent;

    public Task SendAsync(string method, object? arg1)
    {
      throw new NotImplementedException();
    }

    public Task SendAsync(string method, object? arg1, object? arg2)
    {
      throw new NotImplementedException();
    }

    public Task StatusChanged()
    {
      throw new NotImplementedException();
    }
  }
}
