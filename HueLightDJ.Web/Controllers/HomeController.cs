using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HueLightDJ.Web.Models;
using HueLightDJ.Web.Streaming;

namespace HueLightDJ.Web.Controllers
{
  public class HomeController : Controller
  {
    public IActionResult Index()
    {
      var config = StreamingSetup.GetGroupConfigurations();
      return View(config);
    }

    [Route("Preview")]
    public IActionResult Preview()
    {
      return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
