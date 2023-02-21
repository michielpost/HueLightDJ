using Blazored.LocalStorage;
using HueLightDJ.Blazor.Controls.Pages;
using HueLightDJ.Services.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HueLightDJ.Blazor.Controls.Services
{
  public class StorageService
  {
    private readonly ILocalStorageService localStore;

    private readonly string key = "configs";

    public StorageService(Blazored.LocalStorage.ILocalStorageService localStore)
    {
      this.localStore = localStore;
    }
    public async ValueTask<List<GroupConfiguration>> GetAllAsync()
    {
      var result = await localStore.GetItemAsync<List<GroupConfiguration>>(key);
      return result ?? new();
    }

    public async ValueTask<GroupConfiguration?> Get(Guid Id)
    {
      var all = await GetAllAsync();
      return all.Where(x => x.Id == Id).FirstOrDefault();
    }

    public ValueTask SaveAllAsync(List<GroupConfiguration> configs)
    {
      return localStore.SetItemAsync(key, configs);
    }

    public ValueTask<GroupConfiguration> Add(string name)
    {
      var config = new GroupConfiguration { Id = Guid.NewGuid(), Name = name };
      return Save(config);
    }

    internal async ValueTask<GroupConfiguration> Save(GroupConfiguration config)
    {
      var all = await GetAllAsync();

      var existing = all.Where(x => x.Id == config.Id).FirstOrDefault();
      if (existing != null)
        all.Remove(existing);

      all.Insert(0, config);
      await SaveAllAsync(all);

      return config;
    }

    public async Task Delete(Guid id)
    {
      var all = await GetAllAsync();

      var existing = all.Where(x => x.Id == id).FirstOrDefault();
      if (existing != null)
        all.Remove(existing);

      await SaveAllAsync(all);
    }
    
  }
}
