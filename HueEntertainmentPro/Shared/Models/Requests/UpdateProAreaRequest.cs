using ProtoBuf;

namespace HueEntertainmentPro.Shared.Models.Requests
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class UpdateProAreaRequest
  {
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
  }
}
