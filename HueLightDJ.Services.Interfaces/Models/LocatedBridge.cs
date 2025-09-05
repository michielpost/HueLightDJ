using ProtoBuf;

namespace HueLightDJ.Services.Interfaces.Models
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class LocatedBridge
  {
    public required string BridgeId { get; set; }
    public required string IpAddress { get; set; }
    public int? Port { get; set; }

  }
}
