namespace HueEntertainmentPro.Shared.Interfaces
{
  public interface IEventMonitorClient
  {
    Task Subscribed(string message);
    Task Unsubscribed(string message);
    Task Error(string message);
    Task ReceiveEvent(EventData eventData);
  }

  public class EventData
  {
    public string BridgeIp { get; set; } = string.Empty;
    public EventDetails EventDetails { get; set; } = new EventDetails();
  }

  public class EventDetails
  {
    public string? Name { get; set; }
    public string? IdV1 { get; set; }
    public Dictionary<string, object> ExtensionData { get; set; } = new Dictionary<string, object>();
  }
}
