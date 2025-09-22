using HueEntertainmentPro.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace HueEntertainmentPro.Client.Services
{
  public class EventMonitorService : IAsyncDisposable
  {
    private readonly HubConnection _hubConnection;

    public event Action<Guid>? OnSubscribed;
    public event Action<Guid>? OnUnsubscribed;
    public event Action<string>? OnError;
    public event Action<EventData>? OnEventReceived;

    public EventMonitorService(NavigationManager navigationManager)
    {
      _hubConnection = new HubConnectionBuilder()
          .WithUrl($"{navigationManager.BaseUri}eventmonitorhub")
          .WithAutomaticReconnect()
          .Build();

      // Register SignalR client methods
      _hubConnection.On<Guid>(nameof(IEventMonitorClient.Subscribed), id => OnSubscribed?.Invoke(id));
      _hubConnection.On<Guid>(nameof(IEventMonitorClient.Unsubscribed), id => OnUnsubscribed?.Invoke(id));
      _hubConnection.On<string>(nameof(IEventMonitorClient.Error), error => OnError?.Invoke(error));
      _hubConnection.On<EventData>(nameof(IEventMonitorClient.ReceiveEvent), data => OnEventReceived?.Invoke(data));

      _hubConnection.StartAsync();
    }

    public Task SubscribeAsync(Guid bridgeId) => _hubConnection.SendAsync("Subscribe", bridgeId);

    public Task UnsubscribeAsync(Guid bridgeId) => _hubConnection.SendAsync("Unsubscribe", bridgeId);

    public ValueTask DisposeAsync() => _hubConnection.DisposeAsync();
  }
}
