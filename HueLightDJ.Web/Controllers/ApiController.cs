using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueLightDJ.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HueLightDJ.Web.Controllers
{
  [Route("api")]
  [ApiController]
  public class ApiController : ControllerBase
  {
    private readonly IHubService hub;

    public ApiController(IHubService hub)
    {
      this.hub = hub;
    }

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
      EffectService.Beat(hub, intensity);
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
