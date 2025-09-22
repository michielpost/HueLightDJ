using ProtoBuf;

namespace HueLightDJ.Services.Interfaces.Models.Requests
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class BoolRequest
  {
    public BoolRequest()
    {

    }
    public BoolRequest(bool value)
    {
      Value = value;
    }
    public bool Value { get; set; }
  }
}
