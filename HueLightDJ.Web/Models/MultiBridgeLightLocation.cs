using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Models
{
  public class MultiBridgeLightLocation
  {
    public string Bridge { get; set; }
    public string GroupId { get; set; }

    public string Id { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
  }
}
