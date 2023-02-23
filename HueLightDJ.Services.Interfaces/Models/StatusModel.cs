using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Services.Interfaces.Models
{
  public class StatusModel
  {
    public string? Status { get; set; }
    public int bpm { get; set; }
    public bool IsAutoMode { get; set; }
    public bool AutoModeHasRandomEffects { get; set; }

    public bool ShowDisconnect { get; set; }
    public string? CurrentGroup { get; set; }
  }
}
