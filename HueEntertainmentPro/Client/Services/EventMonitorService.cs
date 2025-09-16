using HueEntertainmentPro.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace HueEntertainmentPro.Client.Services
{
  public class EventMonitorService : IAsyncDisposable
  {
    private readonly HubConnection _hubConnection;
    //private readonly EventMonitorClientProxy _clientProxy;

    public event Action<Guid>? OnSubscribed;
    public event Action<Guid>? OnUnsubscribed;
    public event Action<string>? OnError;
    public event Action<EventData>? OnEventReceived;

    public EventMonitorService(NavigationManager navigationManager)
    {
      var url = $"{navigationManager.BaseUri}eventmonitorhub";

      _hubConnection = new HubConnectionBuilder()
          .WithUrl(url)
          .WithAutomaticReconnect()
          .Build();

      // Register typed client methods
      _hubConnection.On<Guid>(nameof(IEventMonitorClient.Subscribed), Subscribed);
      _hubConnection.On<Guid>(nameof(IEventMonitorClient.Unsubscribed), Unsubscribed);
      _hubConnection.On<string>(nameof(IEventMonitorClient.Error), Error);
      _hubConnection.On<EventData>(nameof(IEventMonitorClient.ReceiveEvent), ReceiveEvent);

      _hubConnection.StartAsync();

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
      //await _clientProxy.DisposeAsync();
      await _hubConnection.DisposeAsync();
    }

    // Protected methods to raise events
    public void RaiseSubscribed(Guid message)
    {
      OnSubscribed?.Invoke(message);
    }

    public void RaiseUnsubscribed(Guid message)
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

    // Add a method to start the connection asynchronously
    public async Task StartAsync()
    {
      await _hubConnection.StartAsync();
    }

    public async Task Subscribed(Guid message)
    {
      RaiseSubscribed(message);
      await Task.CompletedTask;
    }

    public async Task Unsubscribed(Guid message)
    {
      RaiseUnsubscribed(message);
      await Task.CompletedTask;
    }

    public async Task Error(string message)
    {
      RaiseError(message);
      await Task.CompletedTask;
    }

    public async Task ReceiveEvent(EventData eventData)
    {
      RaiseEventReceived(eventData);
      await Task.CompletedTask;
    }
  }
}
