using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HueLightDJ.Web.Models;
using HueLightDJ.Web.Streaming;
using Q42.HueApi;
using Q42.HueApi.Models.Bridge;
using Q42.HueApi.Models.Groups;

namespace HueLightDJ.Web.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet]
    public async Task<IActionResult> Index(bool isAdmin)
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
    public async Task<List<Dictionary<string, LightLocation>>> ExportJson([FromRoute]string groupName)
    {
      var locations = await StreamingSetup.GetLocationsAsync(groupName);

      return locations.GroupBy(x => x.Bridge)
        .Select(x => x.ToDictionary(l => l.Id, loc => new LightLocation() { loc.X, loc.Y, 0 })).ToList();

    }

    [HttpGet]
    [Route("fullexport/{groupName}")]
    public Task<List<MultiBridgeLightLocation>> FullExportJson([FromRoute]string groupName)
    {
      return StreamingSetup.GetLocationsAsync(groupName);
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ConnectionConfiguration> Register([FromForm]string ip)
    {
      var hueClient = new LocalHueClient(ip);
      var result = await hueClient.RegisterAsync("HueLightDJ", "Web", generateClientKey: true);

      var allLights = await hueClient.GetLightsAsync();
      string groupId = "GroupId";

      if(allLights.Any())
        groupId = await hueClient.CreateGroupAsync(allLights.Take(10).Select(x => x.Id), "Hue Light DJ group", Q42.HueApi.Models.Groups.RoomClass.Other, Q42.HueApi.Models.Groups.GroupType.Entertainment);

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
