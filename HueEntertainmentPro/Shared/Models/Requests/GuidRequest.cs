using ProtoBuf;

namespace HueEntertainmentPro.Shared.Models.Requests
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class GuidRequest
  {
    public Guid Id { get; set; }
  }
}
