using HueEntertainmentPro.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace HueEntertainmentPro.Client.Services
{
  public class EventMonitorClientProxy : IEventMonitorClient, IAsyncDisposable
  {
    private readonly HubConnection _hubConnection;
    private readonly EventMonitorService _service;

    public EventMonitorClientProxy(EventMonitorService service, string hubUrl)
    {
      _service = service;
      _hubConnection = new HubConnectionBuilder()
          .WithUrl(hubUrl)
          .WithAutomaticReconnect()
          .Build();

      // Register typed client methods
      _hubConnection.On<string>(nameof(IEventMonitorClient.Subscribed), Subscribed);
      _hubConnection.On<string>(nameof(IEventMonitorClient.Unsubscribed), Unsubscribed);
      _hubConnection.On<string>(nameof(IEventMonitorClient.Error), Error);
      _hubConnection.On<EventData>(nameof(IEventMonitorClient.ReceiveEvent), ReceiveEvent);
    }

    // Add a method to start the connection asynchronously
    public async Task StartAsync()
    {
      await _hubConnection.StartAsync();
    }

    public async Task Subscribed(string message)
    {
      _service.RaiseSubscribed(message);
      await Task.CompletedTask;
    }

    public async Task Unsubscribed(string message)
    {
      _service.RaiseUnsubscribed(message);
      await Task.CompletedTask;
    }

    public async Task Error(string message)
    {
      _service.RaiseError(message);
      await Task.CompletedTask;
    }

    public async Task ReceiveEvent(EventData eventData)
    {
      _service.RaiseEventReceived(eventData);
      await Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
      await _hubConnection.DisposeAsync();
    }
  }
}
