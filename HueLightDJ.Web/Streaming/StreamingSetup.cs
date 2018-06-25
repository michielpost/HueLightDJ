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
					public static Ref<TimeSpan?> WaitTime { get; set; } = TimeSpan.FromMilliseconds(500);

					public static async Task<StreamingGroup> SetupAndReturnGroup()
					{
							  //string ip = "192.168.0.4";
							  //string key = "8JwWAj5J1tSsKLxyUOdAkWmcCQFcNc51AKRhxdH9";
							  //string entertainmentKey = "AFFD322C34C993C19503D369481869FD";
							  //var useSimulator = false;


							  //string ip = "10.42.39.194";
							  //string key = "tocjq6GmPJ8KX5DyLDKXQreZE6txQVQ5oBqbYDFn";
							  //string entertainmentKey = "DB088F63639524B5A8CDC8AEEAC9C322";
							  //var useSimulator = false;

							  string ip = "127.0.0.1";
							  string key = "aSimulatedUser";
							  string entertainmentKey = "01234567890123456789012345678901";
							  var useSimulator = true;

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
							  return 1000 * 60 / (int)WaitTime.Value.Value.TotalMilliseconds;
					}

					public static int SetBPM(double bpm)
					{
							  var ms = 1000 / (bpm / 60);
							  WaitTime.Value = TimeSpan.FromMilliseconds(ms);
							  return GetBPM();
					}

					public static int IncreaseBPM(int value)
					{
							  var current = GetBPM();
							  return SetBPM(current + value);
					}
		  }
}
