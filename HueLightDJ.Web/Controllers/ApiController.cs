using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueLightDJ.Web.Streaming;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HueLightDJ.Web.Controllers
{
  [Route("api")]
  [ApiController]
  public class ApiController : ControllerBase
  {
    [HttpPost("setcolors")]
    public void SetColors([FromBody]string[,] matrix)
    {
      ManualControlService.SetColors(matrix);
    }

    [HttpPost("setcolorslist")]
    public void SetColors([FromBody]List<List<string>> matrix)
    {
      ManualControlService.SetColors(matrix);
    }

    [HttpPost("beat")]
    public void Beat([FromBody]double intensity)
    {
      EffectService.Beat(intensity);
    }

    [HttpPost("start-auto")]
    public void StartAuto()
    {
      EffectService.StartAutoMode();
    }

    [HttpPost("stop-auto")]
    public void StopAuto()
    {
      EffectService.StopAutoMode();
    }

    [HttpPost("test")]
    public void Test([FromBody]string test)
    {
    }

    [HttpPost("testempty")]
    public void TestEmpty()
    {
    }
  }
}
