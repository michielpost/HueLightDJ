using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HueLightDJ.Web.Models;
using HueLightDJ.Web.Streaming;
using HueApi;
using HueApi.Models;
using HueApi.BridgeLocator;
using Org.BouncyCastle.Ocsp;
using HueApi.Extensions.cs;

namespace HueLightDJ.Web.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet]
    public IActionResult Index(bool isAdmin)
    {
      ViewBag.IsAdmin = isAdmin;
      return View();
    }

    [HttpGet]
    [Route("Preview")]
    public IActionResult Preview()
    {
      return View();
    }

    [HttpGet]
    [Route("Setup")]
    public async Task<IActionResult> Setup()
    {
      var bridgeLocator = new HttpBridgeLocator();
      var ips = await bridgeLocator.LocateBridgesAsync(TimeSpan.FromSeconds(2));

      return View(ips);
    }

    [HttpGet]
    [Route("Configure")]
    public async Task<IActionResult> Configure()
    {
      var config = await StreamingSetup.GetGroupConfigurationsAsync();
      return View(config);
    }

    [HttpGet]
    [Route("export/{groupName}")]
    public async Task<List<Dictionary<Guid, HuePosition>>> ExportJson([FromRoute]string groupName)
    {
      var locations = await StreamingSetup.GetLocationsAsync(groupName);

      return locations.GroupBy(x => x.Bridge)
        .Select(x => x.ToDictionary(l => l.Id, loc => new HuePosition(loc.X, loc.Y, 0))).ToList();

    }

    [HttpGet]
    [Route("fullexport/{groupName}")]
    public Task<List<MultiBridgeHuePosition>> FullExportJson([FromRoute]string groupName)
    {
      return StreamingSetup.GetLocationsAsync(groupName);
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ConnectionConfiguration> Register([FromForm]string ip)
    {
      var result = await LocalHueApi.RegisterAsync(ip, "HueLightDJ", "Web", generateClientKey: true);

      if (result == null || result.Username == null)
        throw new Exception("No result from bridge");

      var hueClient = new LocalHueApi(ip, result.Username);
      var allLights = await hueClient.GetEntertainmentServicesAsync();

      var createReq = new HueApi.Models.Requests.UpdateEntertainmentConfiguration()
      {
        Metadata = new Metadata() { Name = "Hue Light DJ group " },
        ConfigurationType = EntertainmentConfigurationType.music,
        Locations = new Locations()
      };

      foreach (var light in allLights.Data.Where(x => x.Renderer))
      {
        var lightPosition = new HueServiceLocation
        {
          Service = light.ToResourceIdentifier(),
          Positions = new System.Collections.Generic.List<HuePosition>
           {
             new HuePosition
             {
                X = 0.42, Y = 0.5, Z = 0
             }
          }
        };

        createReq.Locations.ServiceLocations.Add(lightPosition);
      }

      var createResult = await hueClient.CreateEntertainmentConfigurationAsync(createReq);

      var groupId = createResult.Data.First().Rid;

      var connection = new ConnectionConfiguration()
      {
        Ip = ip,
        UseSimulator = false,
        Key = result.Username,
        EntertainmentKey = result.StreamingClientKey,
        GroupId = groupId
      };

      return connection;
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
