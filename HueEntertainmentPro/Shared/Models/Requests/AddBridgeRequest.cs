using ProtoBuf;

namespace HueEntertainmentPro.Shared.Models.Requests
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class AddBridgeRequest
  {
    public string? Name { get; set; }
    public required string BridgeId { get; set; }
    public required string Ip { get; set; }

    public required string Username { get; set; }

    public required string StreamingClientKey { get; set; }
  }
}
