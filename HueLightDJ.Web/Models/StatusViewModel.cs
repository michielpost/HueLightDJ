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
    public bool IsConnected { get; set; }
  }
}
