using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Services.Interfaces.Models
{
  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class StatusModel
  {
    public int Bpm { get; set; }
    public bool IsAutoMode { get; set; }
    public bool AutoModeHasRandomEffects { get; set; }

    public bool ShowDisconnect { get; set; }
    public GroupConfiguration? CurrentGroup { get; set; }

    public List<GroupInfoViewModel> Groups { get; set; } = new();

    //public bool IsConnected => !string.IsNullOrEmpty(CurrentGroup);
  }

  [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
  public class GroupInfoViewModel
  {
    public required string Name { get; set; }

  }
}
