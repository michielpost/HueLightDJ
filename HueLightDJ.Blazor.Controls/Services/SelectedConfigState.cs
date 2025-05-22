using System;

namespace HueLightDJ.Blazor.Controls.Services
{
    public class SelectedConfigState
    {
        public Guid SelectedConfigId { get; private set; }
        public event Action? OnChange;

        public void SetSelectedConfig(Guid configId)
        {
            if (SelectedConfigId != configId)
            {
                SelectedConfigId = configId;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
