using MudBlazor;
using HueLightDJ.Blazor.Controls.Styling; // Add this using
using System;

namespace HueLightDJ.Blazor.Controls.Services
{
    public class ThemeState
    {
        public MudTheme CurrentTheme { get; private set; } = AppThemes.DarkModeTheme; // Default
        public event Action? OnThemeChanged;

        public void SetTheme(MudTheme theme)
        {
            if (CurrentTheme != theme)
            {
                CurrentTheme = theme;
                OnThemeChanged?.Invoke();
            }
        }
    }
}
