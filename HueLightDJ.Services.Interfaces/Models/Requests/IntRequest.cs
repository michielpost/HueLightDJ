using ProtoBuf;

namespace HueLightDJ.Services.Interfaces.Models.Requests
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class IntRequest
  {
    public IntRequest()
    {

    }
    public IntRequest(int value)
    {
      Value = value;
    }
    public int Value { get; set; }
  }
}
