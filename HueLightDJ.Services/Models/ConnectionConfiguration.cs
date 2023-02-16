using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Services.Models
{
  public class ConnectionConfiguration
  {
    public string Ip { get; set; }
    public string Key { get; set; }
    public string? EntertainmentKey { get; set; }
    public Guid GroupId { get; set; }
    public bool UseSimulator { get; set; }
  }
}
