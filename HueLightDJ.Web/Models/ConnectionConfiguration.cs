using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Models
{
  public class ConnectionConfiguration
  {
    public string Ip { get; set; }
    public string Key { get; set; }
    public string EntertainmentKey { get; set; }
    public string GroupId { get; set; }
    public bool UseSimulator { get; set; }
  }
}
