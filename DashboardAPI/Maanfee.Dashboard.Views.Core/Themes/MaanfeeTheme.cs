using MudBlazor;
using MudBlazor.Utilities;

namespace Maanfee.Dashboard.Views.Core
{
    public class Font
    {
        public bool IsRTL { get; set; }

        public string? FontName { get; set; }
    }

    public static class MaanfeeTheme
    {
        public static Font DefaultFont()
        {
            return GetSupportedFonts().First(x => x.IsRTL == SharedLayoutSettings.IsRTL);
        }

        public static List<Font> GetSupportedFonts()
        {
            return new List<Font>
            {
                new Font
                {
                    IsRTL = true,
                    FontName = "BYekan",
                },
                new Font
                {
                    IsRTL = true,
                    FontName = "IranNastaliq",
                },
                new Font
                {
                    IsRTL = true,
                    FontName = "BZiba",
                },
                new Font
                {
                    IsRTL = true,
                    FontName = "IranSans",
                },
                new Font
                {
                    IsRTL = true,
                    FontName = "Titr",
                },
                
                // ********************

                new Font
                {
                    IsRTL = false,
                    FontName = "OpenSans",
                },
                new Font
                {
                    IsRTL = false,
                    FontName = "Playball",
                },
                new Font
                {
                    IsRTL = false,
                    FontName = "Roboto",
                },
                new Font
                {
                    IsRTL = false,
                    FontName = "PlaywriteNZBasic",
                },

            };
        }

        public static string[] CurrentFont(string? NewFont)
        {
            var font = GetSupportedFonts().FirstOrDefault(x => x.IsRTL == SharedLayoutSettings.IsRTL &&
                x.FontName == SharedLayoutSettings.SelectedFont?.FontName);
            if (font != null)
            {
                return new string[] { font.FontName ?? "" };
            }

            return new string[] { DefaultFont().FontName ?? "" };
        }

        public static Typography Typography(string? NewFont) => new Typography()
        {
            Default = new DefaultTypography()
            {
                FontFamily = CurrentFont(NewFont),
                FontSize = ".875rem",
                FontWeight = "400",
                LineHeight = "1.43",
                LetterSpacing = ".01071em"
            },
            H1 = new H1Typography()
            {
                FontFamily = CurrentFont(NewFont),
                FontSize = "6rem",
                FontWeight = "300",
                LineHeight = "1.167",
                LetterSpacing = "-.01562em"
            },
            H2 = new H2Typography()
            {
                FontFamily = CurrentFont(NewFont),
                FontSize = "3.75rem",
                FontWeight = "300",
                LineHeight = "1.2",
                LetterSpacing = "-.00833em"
            },
            H3 = new H3Typography()
            {
                FontFamily = CurrentFont(NewFont),
                FontSize = "3rem",
                FontWeight = "400",
                LineHeight = "1.167",
                LetterSpacing = "0"
            },
            H4 = new H4Typography()
            {
                FontFamily = CurrentFont(NewFont),
                FontSize = "2.125rem",
                FontWeight = "400",
                LineHeight = "1.235",
                LetterSpacing = ".00735em"
            },
            H5 = new H5Typography()
            {
                FontFamily = CurrentFont(NewFont),
                FontSize = "1.5rem",
                FontWeight = "400",
                LineHeight = "1.334",
                LetterSpacing = "0"
            },
            H6 = new H6Typography()
            {
                FontFamily = CurrentFont(NewFont),
                FontSize = "1.25rem",
                FontWeight = "400",
                LineHeight = "1.6",
                LetterSpacing = ".0075em"
            },
            Button = new ButtonTypography()
            {
                FontFamily = CurrentFont(NewFont),
                FontSize = ".875rem",
                FontWeight = "500",
                LineHeight = "1.75",
                LetterSpacing = ".02857em"
            },
            Body1 = new Body1Typography()
            {
                FontFamily = CurrentFont(NewFont),
                FontSize = "1rem",
                FontWeight = "400",
                LineHeight = "1.5",
                LetterSpacing = ".00938em"
            },
            Body2 = new Body2Typography()
            {
                FontFamily = CurrentFont(NewFont),
                FontSize = ".875rem",
                FontWeight = "400",
                LineHeight = "1.43",
                LetterSpacing = ".01071em"
            },
            Caption = new CaptionTypography()
            {
                FontFamily = CurrentFont(NewFont),
                FontSize = ".75rem",
                FontWeight = "400",
                LineHeight = "1.66",
                LetterSpacing = ".03333em"
            },
            Subtitle2 = new Subtitle2Typography()
            {
                FontFamily = CurrentFont(NewFont),
                FontSize = ".875rem",
                FontWeight = "500",
                LineHeight = "1.57",
                LetterSpacing = ".00714em"
            }
        };

        private static LayoutProperties DefaultLayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "3px"
        };

        public static MudTheme ThemeBuilder(MudColor ThemeColor, string? NewFont)
        {
            return new MudTheme()
            {
                PaletteLight = new PaletteLight()
                {
                    Primary = ThemeColor == null ? new PaletteLight().Primary : ThemeColor,
                    AppbarBackground = ThemeColor == null ? new PaletteLight().Primary : ThemeColor,
                    //	Primary = "#1E88E5",
                    Background = "#f2f2f2"/*Colors.Grey.Lighten5*/,
                    //	DrawerBackground = "#FFF",
                    //	DrawerText = "rgba(0,0,0, 0.7)",
                    //	Success = "#007E33",

                },
                //Typography = (SharedLayoutSettings.IsRTL) ? RtlTypography : LtrTypography,
                Typography = Typography(NewFont),
                LayoutProperties = DefaultLayoutProperties
            };
        }

    }
}
