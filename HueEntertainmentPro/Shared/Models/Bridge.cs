using ProtoBuf;

namespace HueEntertainmentPro.Shared.Models
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class Bridge
  {
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? BridgeId { get; set; }
    public string Ip { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

  }
}
