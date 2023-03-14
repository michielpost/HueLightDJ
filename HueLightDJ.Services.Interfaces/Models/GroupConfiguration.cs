using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Services.Interfaces.Models
{
  public class GroupConfiguration
  {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public List<ConnectionConfiguration> Connections { get; set; } = new List<ConnectionConfiguration>();
    public SimpleHuePosition? LocationCenter { get; set; }
    public bool IsAlwaysVisible { get; set; }
    public bool HideDisconnect { get; set; }
  }
}
