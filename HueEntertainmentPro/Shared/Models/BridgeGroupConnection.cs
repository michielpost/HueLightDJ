using ProtoBuf;

namespace HueEntertainmentPro.Shared.Models
{

  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class BridgeGroupConnection
  {
    public Guid Id { get; set; }

    public required Bridge Bridge { get; set; }
    public Guid GroupId { get; set; }

    public string? Name { get; set; }

  }
}
