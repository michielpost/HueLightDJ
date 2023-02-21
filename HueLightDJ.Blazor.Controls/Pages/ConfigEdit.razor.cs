using HueLightDJ.Services.Interfaces.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Blazor.Controls.Pages
{
  public partial class ConfigEdit
  {
    [Parameter]
    public Guid Id { get; set; }

    GroupConfiguration? config;

    protected override async Task OnParametersSetAsync()
    {
      config = await LocalStorageService.Get(Id);
      base.OnParametersSetAsync();
    }

    public async Task Save()
    {
      //var config = await LocalStorageService.Add("HueLightDJ setup");
      //NavManager.NavigateTo($"/config-edit?id={config.Id}");
    }
  }
}
