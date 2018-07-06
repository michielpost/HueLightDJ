using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Models
{
  public class GroupModel
  {
    public string Name { get; set; }
    public IEnumerable<IEnumerable<EntertainmentLight>> Lights { get; set; }

    public GroupModel(string name, IEnumerable<IEnumerable<EntertainmentLight>> lights)
    {
      Name = name;
      Lights = lights;
    }

  }
}
