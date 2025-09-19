
namespace HueEntertainmentPro.Shared.Interfaces
{
  public interface IEventMonitorClient
  {
    Task Subscribed(Guid message);
    Task Unsubscribed(Guid message);
    Task Error(string message);
    Task ReceiveEvent(EventData eventData);
  }

  public class EventData
  {
    public string BridgeIp { get; set; } = string.Empty;
    public EventDetails EventDetails { get; set; } = new EventDetails();
    public DateTimeOffset CreationTime { get; set; }
    public DateTimeOffset SendTime { get; set; }

    public Guid Id { get; set; }
    public string Type { get; set; } = default!;
  }

  public class EventDetails
  {
    public string? Name { get; set; }
    public string? IdV1 { get; set; }
    public Dictionary<string, object> ExtensionData { get; set; } = new Dictionary<string, object>();

  }
}
