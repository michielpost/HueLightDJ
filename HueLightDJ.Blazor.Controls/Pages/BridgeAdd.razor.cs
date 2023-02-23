using Blazored.LocalStorage;
using HueApi.BridgeLocator;
using HueApi.Models;
using HueLightDJ.Services.Interfaces.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Blazor.Controls.Pages
{
  public partial class BridgeAdd
  {
    [Parameter]
    public Guid Id { get; set; }

    GroupConfiguration? config;
    IEnumerable<LocatedBridge> bridges = new List<LocatedBridge>();

    private string ipAdd { get; set; } = string.Empty;

    protected override async Task OnParametersSetAsync()
    {
      config = await LocalStorageService.Get(Id);
      bridges = await HueSetupService.LocateBridgesAsync();
      await base.OnParametersSetAsync();
    }

    public void Select(string ip)
    {
      ipAdd = ip;
    }

    public async Task Save()
    {
      if (config == null)
        return;

      //Get key from hue bridge
      var result = await HueSetupService.RegisterAsync(ipAdd);

      if (result == null || result.Username == null || string.IsNullOrEmpty(result.StreamingClientKey))
        throw new Exception("No result from bridge");

      //Add to config
      if (config == null)
        return;

      config.Connections.Add(new ConnectionConfiguration
      {
        EntertainmentKey = result.StreamingClientKey,
        Ip = result.Ip ?? ipAdd,
        Key = result.Username
      });

      await LocalStorageService.Save(config);
      NavManager.NavigateTo($"/bridge-edit/{config.Id}/{result.Ip}");
    }
  }
}
