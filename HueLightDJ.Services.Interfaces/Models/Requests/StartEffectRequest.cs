using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Services.Interfaces.Models.Requests
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class StartEffectRequest
  {
    public required string TypeName { get; set; }
    public string? ColorHex { get; set; }
    public string? GroupName { get; set; }
    public string? IteratorMode { get; set; }
    public string? SecondaryIteratorMode { get; set; }

  }
}
