using HueEntertainmentPro.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace HueEntertainmentPro.Client.Services
{
  public class EventMonitorService : IAsyncDisposable
  {
    private readonly HubConnection _hubConnection;
    private readonly EventMonitorClientProxy _clientProxy;

    public event Action<string>? OnSubscribed;
    public event Action<string>? OnUnsubscribed;
    public event Action<string>? OnError;
    public event Action<EventData>? OnEventReceived;

    public EventMonitorService(NavigationManager navigationManager)
    {
      var url = $"{navigationManager.BaseUri}eventmonitorhub";

      _hubConnection = new HubConnectionBuilder()
          .WithUrl(url)
          .WithAutomaticReconnect()
          .Build();

      _clientProxy = new EventMonitorClientProxy(this, url);
    }

    public async Task SubscribeAsync(Guid bridgeId)
    {
      await _hubConnection.SendAsync("Subscribe", bridgeId);
    }

    public async Task UnsubscribeAsync(Guid bridgeId)
    {
      await _hubConnection.SendAsync("Unsubscribe", bridgeId);
    }

    public async ValueTask DisposeAsync()
    {
      await _clientProxy.DisposeAsync();
      await _hubConnection.DisposeAsync();
    }

    // Protected methods to raise events
    public void RaiseSubscribed(string message)
    {
      OnSubscribed?.Invoke(message);
    }

    public void RaiseUnsubscribed(string message)
    {
      OnUnsubscribed?.Invoke(message);
    }

    public void RaiseError(string message)
    {
      OnError?.Invoke(message);
    }

    public void RaiseEventReceived(EventData eventData)
    {
      OnEventReceived?.Invoke(eventData);
    }
  }
}
