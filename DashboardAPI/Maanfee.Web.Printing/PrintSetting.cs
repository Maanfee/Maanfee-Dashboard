namespace Maanfee.Web.Printing
{
    public static class PrintSetting
    {
        public static string PageSize { get; set; } = "A4";
        public static string IsLandscape { get; set; } = "false";
        public static string Padding { get; set; } = "padding-10mm";
        public static string Multiple { get; set; } = "1";
        public static string AutoPrint { get; set; } = "Yes";
    }
    
    public struct PageSize
    {
        public static string A3 = "A3";

        public static string A4 = "A4";

        public static string A5 = "A5";

        public static string Letter = "letter";

        public static string Legal = "legal";
    };

    public struct Padding
    {
		public static string Padding0 = "padding-0mm";
		
        public static string Padding5 = "5mm";
		
        public static string Padding10 = "10mm";

        public static string Padding15 = "15mm";

        public static string Padding20 = "20mm";

        public static string Padding25 = "25mm";

		public static string Padding30 = "30mm";
	}
}
