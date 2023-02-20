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
    private readonly EffectService effectService;

    public ApiController(IHubService hub, EffectService effectService)
    {
      this.hub = hub;
      this.effectService = effectService;
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
      effectService.Beat(intensity);
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
