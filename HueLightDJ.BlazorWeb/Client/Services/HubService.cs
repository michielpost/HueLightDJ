using HueLightDJ.Services.Interfaces;
using HueLightDJ.Services.Interfaces.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.ServiceModel.Channels;

namespace HueLightDJ.BlazorWeb.Client.Services
{
  public class HubService : IHubService
  {
    private HubConnection? hubConnection;
    private readonly NavigationManager navigationManager;

    public event EventHandler<string?>? LogMsgEvent;
    public event EventHandler? StatusChangedEvent;
    public event EventHandler<IEnumerable<PreviewModel>>? PreviewEvent;

    public HubService(NavigationManager navigationManager)
    {
      this.navigationManager = navigationManager;
    }
    public async Task OnInitializedAsync()
    {
      var url = $"{navigationManager.BaseUri}previewHub";

      hubConnection = new HubConnectionBuilder()
          .WithUrl(url)
          .Build();

      hubConnection.On<string>("StatusMsg", (user) =>
      {
        var encodedMsg = $"{user}";
        Console.WriteLine(encodedMsg);
      });

      //hubConnection.On("StatusMsg", test);

      await hubConnection.StartAsync();
      await hubConnection.InvokeAsync("Connect");

      await Task.Delay(1500);

      var status = hubConnection.State;
      if(status == HubConnectionState.Connected)
      {

      }
    }

    private void test()
    {
      Console.WriteLine("test");
    }

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
