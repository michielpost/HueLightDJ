using HueLightDJ.Services.Interfaces.Models;
using HueLightDJ.Services.Interfaces.Models.Requests;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MudBlazor.CategoryTypes;

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
    IEnumerable<SimpleEntertainmentGroup>? groups;

    protected override async Task OnParametersSetAsync()
    {
      config = await LocalStorageService.Get(Id);
      if (config == null)
        return;

      bridge = config.Connections.Where(x => x.Ip == Ip).FirstOrDefault();
      if (bridge == null)
        return;

      groups = await HueSetupService.GetEntertainmentGroupsAsync(new HueSetupRequest { Ip = bridge.Ip, Key = bridge.Key });

      await base.OnParametersSetAsync();
    }

    public async Task Remove()
    {
      if (config == null)
        return;

      config.Connections.RemoveAll(x => x.Ip == this.Ip);

      await LocalStorageService.Save(config);
      NavManager.NavigateTo($"/config-edit/{config.Id}");
    }

    public async Task Save()
    {
      if (config == null)
        return;

      await LocalStorageService.Save(config);
      NavManager.NavigateTo($"/config-edit/{config.Id}");
    }

    public async Task Select(Guid groupId)
    {
      if (config == null || bridge == null)
        return;

      bridge.GroupId = groupId;

      await LocalStorageService.Save(config);
      NavManager.NavigateTo($"/config-edit/{config.Id}");
    }

  }
}
