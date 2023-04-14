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

    private string titleEdit { get; set; } = string.Empty;


    protected override async Task OnParametersSetAsync()
    {
      config = await LocalStorageService.Get(Id);
      titleEdit = config?.Name;
      await base.OnParametersSetAsync();
    }

    public async Task Save()
    {
      if(config == null)
        return;

      config.Name = titleEdit;
      config = await LocalStorageService.Save(config);
      NavManager.NavigateTo($"/");
    }

    public async Task Delete()
    {
      if (config == null)
        return;

      await LocalStorageService.Delete(config.Id);
      NavManager.NavigateTo($"/");
    }
  }
}
