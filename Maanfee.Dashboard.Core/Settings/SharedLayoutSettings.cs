
namespace Maanfee.Dashboard.Core
{
    public static class SharedLayoutSettings
    {
        public static bool IsRTL { get; set; }

		public static bool IsDarkMode { get; set; }

        public static string ThemeColor { get; set; }
    }

    public class LayoutSettings
	{
		public bool IsRTL { get; set; }

		public bool IsDarkMode { get; set; }

        public string ThemeColor { get; set; }
    }
}
