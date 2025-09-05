using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueEntertainmentPro.Shared.Models.Requests
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class AddBridgeRequest
  {
    public string? Name { get; set; }
    public required string BridgeId { get; set; }
    public required string Ip { get; set; }

    public required string Username { get; set; }

    public required string StreamingClientKey { get; set; }
  }
}
