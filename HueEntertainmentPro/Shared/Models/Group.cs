using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueEntertainmentPro.Shared.Models
{

  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class Group
  {
    public Guid Id { get; set; }
  }
}
