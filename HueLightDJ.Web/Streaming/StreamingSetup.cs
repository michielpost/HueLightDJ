using Microsoft.Extensions.Configuration;
using Q42.HueApi.Streaming;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightDJ.Web.Streaming
{
  public static class StreamingSetup
  {
    public static StreamingGroup StreamingGroup { get; set; }
    public static StreamingHueClient StreamingHueClient { get; set; }
    public static List<EntertainmentLayer> Layers { get; set; }
    private static int BPM { get; set; } = 120;
    public static Ref<TimeSpan?> WaitTime { get; set; } = TimeSpan.FromMilliseconds(500);

    public static async Task<StreamingGroup> SetupAndReturnGroup()
    {
      var configSection = Startup.Configuration.GetSection("HueSetup");
      string ip = configSection.GetValue<string>("ip");
      string key = configSection.GetValue<string>("key");
      string entertainmentKey = configSection.GetValue<string>("entertainmentKey");
      bool useSimulator = configSection.GetValue<bool>("useSimulator"); 

      StreamingGroup = null;
      Layers = null;

      //Initialize streaming client
      StreamingHueClient = new StreamingHueClient(ip, key, entertainmentKey);

      //Get the entertainment group
      var all = await StreamingHueClient.LocalHueClient.GetEntertainmentGroups();
      var group = all.FirstOrDefault();

      if (group == null)
        throw new Exception("No Entertainment Group found. Create one using the Q42.HueApi.UniversalWindows.Sample");
      else
        Console.WriteLine($"Using Entertainment Group {group.Id}");

      //Create a streaming group
      var stream = new StreamingGroup(group.Locations);
      stream.IsForSimulator = useSimulator;


      //Connect to the streaming group
      await StreamingHueClient.Connect(group.Id, simulator: useSimulator);

      //Start auto updating this entertainment group
      StreamingHueClient.AutoUpdate(stream, 50);


      StreamingGroup = stream;
      var baseLayer = stream.GetNewLayer(isBaseLayer: true);
      var effectLayer = stream.GetNewLayer(isBaseLayer: false);
      Layers = new List<EntertainmentLayer>() { baseLayer, effectLayer };

      //Optional: calculated effects that are placed on this layer
      baseLayer.AutoCalculateEffectUpdate();
      effectLayer.AutoCalculateEffectUpdate();

      return stream;
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
