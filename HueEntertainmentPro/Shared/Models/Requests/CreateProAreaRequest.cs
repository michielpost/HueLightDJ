using ProtoBuf;

namespace HueEntertainmentPro.Shared.Models.Requests
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class CreateProAreaRequest
  {
    public string Name { get; set; } = string.Empty;
  }
}
