using HueLightDJ.Web.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Q42.HueApi;
using Q42.HueApi.Models.Groups;
using Q42.HueApi.Streaming;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Streaming
{
  public static class StreamingSetup
  {
    private static List<StreamingGroup> StreamingGroups { get; set; } = new List<StreamingGroup>();
    private static List<LightDJStreamingHueClient> StreamingHueClients { get; set; } = new List<LightDJStreamingHueClient>();
    public static List<EntertainmentLayer> Layers { get; set; }
    private static int BPM { get; set; } = 120;
    public static Ref<TimeSpan> WaitTime { get; set; } = TimeSpan.FromMilliseconds(500);

    

    public static GroupConfiguration CurrentConnection { get; set; }

    private static string _groupId;
    private static CancellationTokenSource _cts;

    public static async Task<List<MultiBridgeLightLocation>> GetLocationsAsync(string groupName)
    {
      var configSection = GetGroupConfigurations();
      var currentGroup = configSection.Where(x => x.Name == groupName).FirstOrDefault();

      var locations = new List<MultiBridgeLightLocation>();

      foreach (var bridgeConfig in currentGroup.Connections)
      {
        var localClient = new LocalHueClient(bridgeConfig.Ip, bridgeConfig.Key);
        var group = await localClient.GetGroupAsync(bridgeConfig.GroupId);

        if (group.Type != GroupType.Entertainment)
          continue;

        locations.AddRange(group.Locations.Select(x => new MultiBridgeLightLocation()
        {
          Bridge = bridgeConfig.Ip,
          GroupId = bridgeConfig.GroupId,
          Id = x.Key,
          X = x.Value.X,
          Y = x.Value.Y
        }));
      }

      return locations;
    }

    public static async Task SetLocations(List<MultiBridgeLightLocation> locations)
    {
      var configSection = GetGroupConfigurations();

      var grouped = locations.GroupBy(x => x.Bridge);

      foreach(var group in grouped)
      {
        var ip = group.Key;
        var groupId = group.First().GroupId;
        var config = configSection.SelectMany(x => x.Connections).Where(x => x.Ip == ip && x.GroupId == groupId).FirstOrDefault();
        if(config != null)
        {
          var client = new LocalHueClient(config.Ip, config.Key);
          var bridgeLocations = group.ToDictionary(x => x.Id, l => new LightLocation() { l.X, l.Y, 0 });
          await client.UpdateGroupLocationsAsync(groupId, bridgeLocations);
        }
      }
    }

    public static async Task<List<StreamingGroup>> SetupAndReturnGroupAsync(string groupName)
    {
      var configSection = GetGroupConfigurations();
      var currentGroup = configSection.Where(x => x.Name == groupName).FirstOrDefault();
      bool demoMode = currentGroup.Name == "DEMO";
      bool useSimulator = demoMode ? true : currentGroup.Connections.First().UseSimulator;

      //Disconnect any current connections
      Disconnect();
      _cts = new CancellationTokenSource();

      foreach (var bridgeConfig in currentGroup.Connections)
      {
        //Initialize streaming client
        var client = new LightDJStreamingHueClient(bridgeConfig.Ip, bridgeConfig.Key, bridgeConfig.EntertainmentKey, demoMode);

        //Get the entertainment group
        Dictionary<string, LightLocation> locations = null;
        if (demoMode)
        {
          string demoJson = await File.ReadAllTextAsync($"demoLocations{bridgeConfig.GroupId}.json");
          locations = JsonConvert.DeserializeObject<Dictionary<string, LightLocation>>(demoJson);
          _groupId = bridgeConfig.GroupId;
        }
        else
        {
          var all = await client.LocalHueClient.GetEntertainmentGroups();
          var group = all.Where(x => x.Id == bridgeConfig.GroupId).FirstOrDefault();

          if (group == null)
            throw new Exception($"No Entertainment Group found with id {bridgeConfig.GroupId}. Create one using the Philips Hue App or the Q42.HueApi.UniversalWindows.Sample");
          else
          {
            Console.WriteLine($"Using Entertainment Group {group.Id}");
            _groupId = group.Id;
          }

          locations = group.Locations;
        }

        //Create a streaming group
        var stream = new StreamingGroup(locations);
        stream.IsForSimulator = useSimulator;


        //Connect to the streaming group
        if (!demoMode)
          await client.Connect(_groupId, simulator: useSimulator);

        //Start auto updating this entertainment group
        client.AutoUpdate(stream, _cts.Token, 50, onlySendDirtyStates: true);

        StreamingHueClients.Add(client);
        StreamingGroups.Add(stream);
       
      }

      var baseLayer = GetNewLayer(isBaseLayer: true);
      var effectLayer = GetNewLayer(isBaseLayer: false);

      Layers = new List<EntertainmentLayer>() { baseLayer, effectLayer };
      CurrentConnection = currentGroup;

      //Optional: calculated effects that are placed on this layer
      baseLayer.AutoCalculateEffectUpdate(_cts.Token);
      effectLayer.AutoCalculateEffectUpdate(_cts.Token);

      return StreamingGroups;
    }

    public static List<GroupConfiguration> GetGroupConfigurations()
    {
      return Startup.Configuration.GetSection("HueSetup").Get<List<GroupConfiguration>>();
    }

    public static void SetBrightnessFilter(double value)
    {
      foreach (var stream in StreamingGroups)
      {
        stream.BrightnessFilter = value;
      }
    }

    private static EntertainmentLayer GetNewLayer(bool isBaseLayer = false)
    {
      var layer = new EntertainmentLayer(isBaseLayer);
      foreach (var stream in StreamingGroups)
      {
        var all = stream.GetNewLayer(isBaseLayer);
        layer.AddRange(all);
      }
      return layer;
    }

    public static void Disconnect()
    {
      foreach (var client in StreamingHueClients)
      {
        client.LocalHueClient.SetStreamingAsync(_groupId, active: false);
      }

      EffectService.CancelAllEffects();
      if (_cts != null)
        _cts.Cancel();

      Layers = null;
      StreamingHueClients.Clear();
      StreamingGroups.Clear();
      CurrentConnection = null;
     
    }

    public async static Task<bool> IsStreamingActive()
    {
      //Optional: Check if streaming is currently active
      var bridgeInfo = await StreamingHueClients.First().LocalHueClient.GetBridgeAsync();
      Console.WriteLine(bridgeInfo.IsStreamingActive ? "Streaming is active" : "Streaming is not active");

      return bridgeInfo.IsStreamingActive;

    }

    public static int GetBPM()
    {
      return BPM;
    }

    public static int SetBPM(int bpm)
    {
      BPM = bpm;
      WaitTime.Value = TimeSpan.FromMilliseconds((60 * 1000) / bpm);
      return GetBPM();
    }

    public static int IncreaseBPM(int value)
    {
      return SetBPM(BPM + value);
    }
  }
}
