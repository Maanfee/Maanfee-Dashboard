using MudBlazor;
using MudBlazor.Utilities;

namespace Maanfee.Dashboard.Views.Core
{
    public static class MaanfeeThemeColors
    {
        public static MudColor PrimaryColor = new PaletteLight().Primary;

        public static readonly List<MudColor> SupportedThemeColors = new()
        {
            PrimaryColor,
        Colors.Amber.Default,
        Colors.Blue.Default,
        Colors.BlueGray.Default,
        Colors.Brown.Default,
        Colors.Cyan.Default,
        Colors.DeepOrange.Default,
        Colors.DeepPurple.Default,
        Colors.Gray.Default,
        Colors.Green.Default,
        Colors.Indigo.Default,
        Colors.LightBlue.Default,
        Colors.LightGreen.Default,
        Colors.Lime.Default,
        Colors.Orange.Default,
        Colors.Pink.Default,
        Colors.Purple.Default,
        Colors.Red.Default,
        Colors.Teal.Default,
            Colors.Yellow.Darken1,
        };
    }
}
