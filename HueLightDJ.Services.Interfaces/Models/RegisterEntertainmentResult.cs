using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Services.Interfaces.Models
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class RegisterEntertainmentResult
  {
    public string? Ip { get; set; }

    public string? Username { get; set; }

    public string? StreamingClientKey { get; set; }
  }
}
