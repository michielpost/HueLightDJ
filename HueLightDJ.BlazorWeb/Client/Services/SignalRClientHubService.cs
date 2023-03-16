using HueLightDJ.Blazor.Controls.Services;
using HueLightDJ.Services.Interfaces;
using HueLightDJ.Services.Interfaces.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.ServiceModel.Channels;

namespace HueLightDJ.BlazorWeb.Client.Services
{
  public class SignalRClientHubService : HubService
  {
    private HubConnection? hubConnection;
    private readonly NavigationManager navigationManager;

    public SignalRClientHubService(NavigationManager navigationManager) : base()
    {
      this.navigationManager = navigationManager;
    }
    public async Task OnInitializedAsync()
    {
      var url = $"{navigationManager.BaseUri}previewHub";

      hubConnection = new HubConnectionBuilder()
          .WithUrl(url)
          .Build();

      hubConnection.On<string>("StatusMsg", (msg) =>
      {
        this.SendAsync(msg);
      });

      hubConnection.On("StatusChanged", () =>
      {
        this.StatusChanged();
      });

      hubConnection.On<List<PreviewModel>>("preview", (list) =>
      {
        this.SendPreview(list);
      });

      await hubConnection.StartAsync();
      //await hubConnection.InvokeAsync("Connect");

      //await Task.Delay(1500);

      //var status = hubConnection.State;
      //if(status == HubConnectionState.Connected)
      //{

      //}
    }
    
  }
}
