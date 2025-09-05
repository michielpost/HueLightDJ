using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using HueEntertainmentPro.Client.Extensions;
using HueEntertainmentPro.Client.Services;
using HueEntertainmentPro.Shared.Interfaces;
using HueLightDJ.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using ProtoBuf.Grpc.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddFluentUIComponents();

builder.Services.AddSingleton<IHubService, SignalRClientHubService>();

builder.Services.AddHttpClient("ServerAPI",
  client =>
  {
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
  })
  .AddHttpMessageHandler(() => new GrpcWebHandler(GrpcWebMode.GrpcWeb));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
.CreateClient("ServerAPI")
);

builder.Services.AddGrpcService<IBridgeDataService>();
builder.Services.AddGrpcService<IProAreaDataService>();
builder.Services.AddGrpcService<IHueSetupService>();
builder.Services.AddGrpcService<ILightDJService>();

await builder.Build().RunAsync();
