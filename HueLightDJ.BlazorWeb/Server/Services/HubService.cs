using HueLightDJ.Services.Interfaces;
using HueLightDJ.Services.Interfaces.Models;

namespace HueLightDJ.BlazorWeb.Server.Services
{
  public class HubService : IHubService
  {
    public event EventHandler<string?>? LogMsgEvent;
    public event EventHandler? StatusChangedEvent;
    public event EventHandler<IEnumerable<PreviewModel>>? PreviewEvent;

    public Task SendAsync(string method, params object?[] arg1)
    {
      throw new NotImplementedException();
    }

    public Task SendPreview(IEnumerable<PreviewModel> list)
    {
      throw new NotImplementedException();
    }

    public Task StatusChanged()
    {
      throw new NotImplementedException();
    }
  }
}
