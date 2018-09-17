using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Models
{
  public class StatusViewModel
  {
    public string Status { get; set; }
    public int bpm { get; set; }
    public bool IsAutoMode { get; set; }
    public bool AutoModeHasRandomEffects { get; set; }

    public bool ShowDisconnect { get; internal set; }
    public List<string> GroupNames { get; internal set; }
    public string CurrentGroup { get; set; }
  }
}
