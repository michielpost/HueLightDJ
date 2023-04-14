using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Services.Interfaces.Models
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class SimpleEntertainmentGroup
  {
    public Guid Id { get; set; } = default!;

    public string? Name { get; set; }

    public int LightCount { get; set; }
  }
}
