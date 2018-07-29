using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
    public static StreamingGroup StreamingGroup { get; set; }
    public static LightDJStreamingHueClient StreamingHueClient { get; set; }
    public static List<EntertainmentLayer> Layers { get; set; }
    private static int BPM { get; set; } = 120;
    public static Ref<TimeSpan?> WaitTime { get; set; } = TimeSpan.FromMilliseconds(500);

    private static string _groupId;
    private static CancellationTokenSource _cts;

    public static async Task<StreamingGroup> SetupAndReturnGroup(bool demoMode = false)
    {
      var configSection = Startup.Configuration.GetSection("HueSetup");
      string ip = configSection.GetValue<string>("ip");
      string key = configSection.GetValue<string>("key");
      string entertainmentKey = configSection.GetValue<string>("entertainmentKey");
      bool useSimulator = demoMode ? true : configSection.GetValue<bool>("useSimulator");

      EffectService.CancelAllEffects();
      if (_cts != null)
        _cts.Cancel();
      _cts = new CancellationTokenSource();

      StreamingGroup = null;
      Layers = null;

      //Initialize streaming client
      StreamingHueClient = new LightDJStreamingHueClient(ip, key, entertainmentKey, demoMode);

      //Get the entertainment group
      Dictionary<string, LightLocation> locations = null;
      if (demoMode)
      {
        string demoJson = File.ReadAllText("demoLocations.json");
        locations = JsonConvert.DeserializeObject<Dictionary<string, LightLocation>>(demoJson);
        _groupId = "1";
      }
      else
      {
        var all = await StreamingHueClient.LocalHueClient.GetEntertainmentGroups();
        var group = all.FirstOrDefault();

        if (group == null)
          throw new Exception("No Entertainment Group found. Create one using the Philips Hue App or the Q42.HueApi.UniversalWindows.Sample");
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
      if(!demoMode)
        await StreamingHueClient.Connect(_groupId, simulator: useSimulator);

      //Start auto updating this entertainment group
      StreamingHueClient.AutoUpdate(stream, _cts.Token, 50);


      StreamingGroup = stream;
      var baseLayer = stream.GetNewLayer(isBaseLayer: true);
      var effectLayer = stream.GetNewLayer(isBaseLayer: false);
      Layers = new List<EntertainmentLayer>() { baseLayer, effectLayer };

      //Optional: calculated effects that are placed on this layer
      baseLayer.AutoCalculateEffectUpdate(_cts.Token);
      effectLayer.AutoCalculateEffectUpdate(_cts.Token);

      return stream;
    }

    public static void Disconnect()
    {
      StreamingHueClient.LocalHueClient.SetStreamingAsync(_groupId, active: false);
    }

    public async static Task<bool> IsStreamingActive()
    {
      //Optional: Check if streaming is currently active
      var bridgeInfo = await StreamingHueClient.LocalHueClient.GetBridgeAsync();
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
