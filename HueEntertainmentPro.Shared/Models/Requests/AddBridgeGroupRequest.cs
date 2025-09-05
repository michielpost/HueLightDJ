using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueEntertainmentPro.Shared.Models.Requests
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class AddBridgeGroupRequest
  {
    public string? Name { get; set; }
    public required Guid? ProAreaId { get; set; }
    public required Guid? BridgeId { get; set; }
    public required Guid? GroupId { get; set; }
  }
}
