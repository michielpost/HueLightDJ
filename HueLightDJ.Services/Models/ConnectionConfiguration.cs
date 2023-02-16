using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Services.Models
{
  public class ConnectionConfiguration
  {
    public required string Ip { get; set; }
    public required string Key { get; set; }
    public required string EntertainmentKey { get; set; }
    public Guid GroupId { get; set; }
    public bool UseSimulator { get; set; }
  }
}
