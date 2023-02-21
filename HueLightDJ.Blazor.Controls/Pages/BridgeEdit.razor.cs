using HueApi.Models;
using HueLightDJ.Services.Interfaces.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HueLightDJ.Blazor.Controls.Pages
{
  public partial class BridgeEdit
  {
    [Parameter]
    public Guid Id { get; set; }

    [Parameter]
    public string Ip { get; set; }

    GroupConfiguration? config;
    ConnectionConfiguration? bridge;
    List<EntertainmentConfiguration>? groups;

    protected override async Task OnParametersSetAsync()
    {
      config = await LocalStorageService.Get(Id);
      if (config == null)
        return;

      bridge = config.Connections.Where(x => x.Ip == Ip).FirstOrDefault();
      if (bridge == null)
        return;

      groups = await HueSetupService.GetEntertainmentGroupsAsync(bridge.Ip, bridge.Key);

      await base.OnParametersSetAsync();
    }

    public async Task Save()
    {
      //var config = await LocalStorageService.Add("HueLightDJ setup");
      //NavManager.NavigateTo($"/config-edit?id={config.Id}");
    }

  }
}
