using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueLightDJ.Web.Streaming;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HueLightDJ.Web.Hubs
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
  }
}
