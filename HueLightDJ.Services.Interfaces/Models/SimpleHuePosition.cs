using ProtoBuf;

namespace HueLightDJ.Services.Interfaces.Models
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class SimpleHuePosition
  {
    public double X { get; set; }

    public double Y { get; set; }

    public double Z { get; set; }
  }
}
