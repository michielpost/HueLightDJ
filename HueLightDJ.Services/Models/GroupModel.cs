using HueApi.Entertainment.Extensions;
using HueApi.Entertainment.Models;
using System.Collections.Generic;
using System.Linq;

namespace HueLightDJ.Services.Models
{
  public class GroupModel
  {
    public string Name { get; set; }
    public List<IEnumerable<IEnumerable<EntertainmentLight>>> Lights { get; set; } = new List<IEnumerable<IEnumerable<EntertainmentLight>>>();

    /// <summary>
    /// Indicates the maximum number of effects this group can handle on the same time.
    /// Should not be more than the number of items in the first IEnumerable
    /// This is based on your own judgement. How is this group split up? What looks good?
    /// Change it and test it.
    /// </summary>
    public int MaxEffects => Lights.Count;

    public GroupModel(string name, IEnumerable<IEnumerable<EntertainmentLight>> lights)
    {
      Name = name;
      Lights.Add(lights);
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
      Lights = lights.ChunkByGroupNumber(maxEffects).ToList();
    }

    /// <summary>
    /// Multi effect constructor
    /// </summary>
    /// <param name="name"></param>
    /// <param name="multiEffectLights"></param>
    public GroupModel(string name, IEnumerable<IEnumerable<IEnumerable<EntertainmentLight>>> multiEffectLights)
    {
      Name = name;
      Lights = multiEffectLights.ToList();
    }

  }
}
