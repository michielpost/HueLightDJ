using ProtoBuf;
using System;

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
