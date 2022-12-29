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
        public static string Padding10 = "padding-10mm";

        public static string Padding15 = "padding-15mm";

        public static string Padding20 = "padding-20mm";

        public static string Padding25 = "padding-25mm";
    }
}
