using ProtoBuf;
using System.Collections.Generic;

namespace HueLightDJ.Services.Interfaces.Models
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class EntertainmentGroupResult
  {
    public IEnumerable<SimpleEntertainmentGroup> Groups { get; set; } = new List<SimpleEntertainmentGroup>();

    public string? ErrorMessage { get; set; }
  }
}
