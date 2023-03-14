using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
