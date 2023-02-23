using HueLightDJ.Services.Interfaces;
using HueLightDJ.Services.Interfaces.Models;

namespace HueLightDJ.BlazorWeb.Client.Services
{
  public class LightDJService : ILightDJService
  {
    public void Beat(double intensity)
    {
      throw new NotImplementedException();
    }

    public Task Connect(GroupConfiguration config)
    {
      throw new NotImplementedException();
    }

    public Task Disconnect()
    {
      throw new NotImplementedException();
    }

    public Task<EffectsVM> GetEffects()
    {
      throw new NotImplementedException();
    }

    public Task<StatusModel> GetStatus()
    {
      throw new NotImplementedException();
    }

    public Task IncreaseBPM(int value)
    {
      throw new NotImplementedException();
    }

    public Task SetAutoRandomMode(bool value)
    {
      throw new NotImplementedException();
    }

    public Task SetBPM(int value)
    {
      throw new NotImplementedException();
    }

    public void SetBri(double value)
    {
      throw new NotImplementedException();
    }

    public void SetColors(string[,] matrix)
    {
      throw new NotImplementedException();
    }

    public void SetColorsList(List<List<string>> matrix)
    {
      throw new NotImplementedException();
    }

    public Task StartAutoMode()
    {
      throw new NotImplementedException();
    }

    public void StartEffect(string typeName, string? colorHex)
    {
      throw new NotImplementedException();
    }

    public void StartGroupEffect(string typeName, string? colorHex, string groupName, string iteratorMode, string secondaryIteratorMode)
    {
      throw new NotImplementedException();
    }

    public void StartRandom()
    {
      throw new NotImplementedException();
    }

    public Task StopAutoMode()
    {
      throw new NotImplementedException();
    }

    public Task StopEffects()
    {
      throw new NotImplementedException();
    }

    public Task ToggleAutoRandomMode()
    {
      throw new NotImplementedException();
    }
  }
}
