using MudBlazor;
using MudBlazor.Utilities;

namespace HueLightDJ.Blazor.Controls.Styling
{
    public static class AppThemes
    {
        public static MudTheme DarkModeTheme { get; } = new MudTheme()
        {
            PaletteDark = new PaletteDark()
            {
                Primary = Colors.Blue.Lighten1,
                Secondary = Colors.Green.Accent4,
                AppbarBackground = new MudColor("#1E1E1E"), // Example, adjust as needed
                Background = Colors.Gray.Darken4, // Example: "#121212" (standard dark)
                Surface = Colors.Gray.Darken3, // Cards, paper
                DrawerBackground = Colors.Gray.Darken3,
                TextPrimary = Colors.Shades.White,
                TextSecondary = Colors.Gray.Lighten1,
                ActionDefault = Colors.Gray.Lighten1,
            },
            // Ensure Palette is set for a dark theme, even if PaletteDark is primary
            Palette = new PaletteLight() // Or a copy of PaletteDark if MudBlazor handles it
            {
                Primary = Colors.Blue.Lighten1, // Match PaletteDark for consistency if this theme is always dark
                Secondary = Colors.Green.Accent4,
                AppbarBackground = new MudColor("#1E1E1E"),
                Background = Colors.Gray.Darken4,
                Surface = Colors.Gray.Darken3,
                DrawerBackground = Colors.Gray.Darken3,
                TextPrimary = Colors.Shades.White,
                TextSecondary = Colors.Gray.Lighten1,
                ActionDefault = Colors.Gray.Lighten1,
            }
        };

        public static MudTheme LightModeTheme { get; } = new MudTheme()
        {
            Palette = new PaletteLight()
            {
                Primary = Colors.Blue.Default,
                Secondary = Colors.Green.Accent7,
                AppbarBackground = Colors.Blue.Darken2,
                Background = Colors.Gray.Lighten5, // Standard light background
                Surface = Colors.Shades.White,
                DrawerBackground = Colors.Shades.White,
                TextPrimary = Colors.Gray.Darken3,
                TextSecondary = Colors.Gray.Darken1,
            }
        };

        public static MudTheme OceanTheme { get; } = new MudTheme() // Example Theme 1
        {
            Palette = new PaletteLight()
            {
                Primary = Colors.Teal.Default,
                Secondary = Colors.Cyan.Accent7,
                AppbarBackground = Colors.Teal.Darken2,
                Background = Colors.LightBlue.Lighten5,
                Surface = Colors.Shades.White,
            }
        };

        public static MudTheme ForestTheme { get; } = new MudTheme() // Example Theme 2
        {
             PaletteDark = new PaletteDark() // A dark variant
            {
                Primary = Colors.Green.Default,
                Secondary = Colors.LightGreen.Accent4,
                AppbarBackground = Colors.Green.Darken3,
                Background = Colors.Brown.Darken4,
                Surface = Colors.Green.Darken2,
                TextPrimary = Colors.Shades.White,
            },
            Palette = new PaletteLight() // Copy for consistency if primarily dark
            {
                Primary = Colors.Green.Default,
                Secondary = Colors.LightGreen.Accent4,
                AppbarBackground = Colors.Green.Darken3,
                Background = Colors.Brown.Darken4,
                Surface = Colors.Green.Darken2,
                TextPrimary = Colors.Shades.White,
            }
        };

        public static MudTheme SunsetTheme { get; } = new MudTheme() // Example Theme 3
        {
            Palette = new PaletteLight()
            {
                Primary = Colors.Orange.Default,
                Secondary = Colors.Red.Accent4,
                AppbarBackground = Colors.Orange.Darken3,
                Background = Colors.Yellow.Lighten5,
                Surface = Colors.Orange.Lighten4,
            }
        };
    }
}
