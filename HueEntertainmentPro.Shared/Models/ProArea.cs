using HueLightDJ.Services.Interfaces.Models;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueEntertainmentPro.Shared.Models
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class ProArea
  {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public List<BridgeGroupConnection> Connections { get; set; } = new List<BridgeGroupConnection>();

   // public SimpleHuePosition? LocationCenter { get; set; }
   // public bool IsAlwaysVisible { get; set; }
  //  public bool HideDisconnect { get; set; }
  }
}
