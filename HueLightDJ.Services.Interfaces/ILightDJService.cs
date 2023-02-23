using HueApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using HueLightDJ.Services.Interfaces.Models;

namespace HueLightDJ.Services.Interfaces
{
  public interface ILightDJService
  {
    Task Connect(GroupConfiguration config);

    Task<StatusModel> GetStatus();

    Task<EffectsVM> GetEffects();

    void StartEffect(string typeName, string? colorHex);

    void StartGroupEffect(string typeName, string? colorHex, string groupName, string iteratorMode, string secondaryIteratorMode);

    Task IncreaseBPM(int value);

    Task SetBPM(int value);

    void SetBri(double value);

    void StartRandom();

    Task StartAutoMode();

    Task StopAutoMode();

    Task SetAutoRandomMode(bool value);

    [Obsolete]
    Task ToggleAutoRandomMode();

    Task StopEffects();

    void SetColors(string[,] matrix);
    void SetColorsList(List<List<string>> matrix);
    void Beat(double intensity);

    Task Disconnect();
  }
}
