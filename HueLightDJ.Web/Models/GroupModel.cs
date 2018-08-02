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

    /// <summary>
    /// Indicates the maximum number of effects this group can handle on the same time.
    /// Should not be more than the number of items in the first IEnumerable
    /// This is based on your own judgement. How is this group split up? What looks good?
    /// Change it and test it.
    /// </summary>
    public int MaxEffects { get; set; } = 1;

    public GroupModel(string name, IEnumerable<IEnumerable<EntertainmentLight>> lights)
    {
      Name = name;
      Lights = lights;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="lights"></param>
    /// <param name="maxEffects">Indicates the maximum number of effects this group can handle on the same time.
    /// Should not be more than the number of items in the first IEnumerable</param>
    public GroupModel(string name, IEnumerable<IEnumerable<EntertainmentLight>> lights, int maxEffects)
    {
      Name = name;
      Lights = lights;
      MaxEffects = maxEffects;
    }

  }
}
