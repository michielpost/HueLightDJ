using ProtoBuf;

namespace HueLightDJ.Services.Interfaces.Models.Requests
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class DoubleRequest
  {
    public DoubleRequest()
    {

    }
    public DoubleRequest(double value)
    {
      Value = value;
    }
    public double Value { get; set; }
  }
}
