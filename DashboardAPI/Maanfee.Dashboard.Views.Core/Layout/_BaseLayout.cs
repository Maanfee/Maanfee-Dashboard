using Maanfee.Dashboard.Views.Core.Services;
using Maanfee.JsInterop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Globalization;
using System.Reflection;

namespace Maanfee.Dashboard.Views.Core
{
    public partial class _BaseLayout : LayoutComponentBase, IDisposable
    {
        [Inject]
        protected HttpClient? Http { get; set; }

        [Inject]
        protected NavigationManager? Navigation { get; set; }

        #region - Authentication -

        [Inject]
        protected AccountStateContainer? AccountStateContainer { get; set; }

        [Inject]
        protected IAuthorizationService? AuthorizationService { get; set; }


        [CascadingParameter]
        protected Task<AuthenticationState>? AuthenticationState { get; set; }

        #endregion

        [Inject]
        protected CustomAuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        // JWT
        [Inject]
        protected LocalConfigurationService? LocalConfiguration { get; set; }

        // JWT
        [Inject]
        protected LocalStorage? LocalStorage { get; set; }

        // JWT
        [Inject]
        protected JwtAuthenticationStateProvider? JwtAuthenticationStateProvider { get; set; }

        [Inject]
        protected IDialogService? Dialog { get; set; }

        [Inject]
        protected Fullscreen? Fullscreen { get; set; }

        [Inject] 
        protected TableConfigurationService? TableConfiguration { get; set; }

        // *****************************************

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            AccountStateContainer?.OnChange += StateHasChanged;

            SharedLayoutSettings.Theme = MaanfeeTheme.ThemeBuilder(SharedLayoutSettings.Theme.PaletteLight.Primary
                , SharedLayoutSettings.SelectedFont?.FontName);

            SnakbarDirectionConfiguration();
        }

        // ******************************************************

        private void SnakbarDirectionConfiguration()
        {
            Snackbar?.Configuration.PositionClass = (SharedLayoutSettings.IsRTL) ? Defaults.Classes.Position.BottomStart : Defaults.Classes.Position.BottomEnd;
        }

        public void Dispose()
        {
            AccountStateContainer?.OnChange -= StateHasChanged;
        }

        // *****************************************

        #region - MudTable Config -

        protected bool _IsTableDense { get; set; } = true;

        protected bool _IsTableFixedHeader { get; set; } = true;

        protected bool _IsTableScroll { get; set; } = true;

        public static string TableHeight { get; set; } = TableConfigurationService.InitTableHeight;

        public static string TableHeightLarge { get; set; } = TableConfigurationService.InitTableHeightLarge;

        protected async void OnScrollChanged()
        {
            _IsTableScroll = _IsTableScroll ? false : true;

            TableHeight = TableConfiguration!.SetHeight(SharedLayoutSettings.IsRTL, await Fullscreen!.IsFullscreenAsync(), _IsTableScroll);

            StateHasChanged();
        }

        #endregion

        #region - Snackbar Config -

        [Inject]
        protected ISnackbar? Snackbar { get; set; }

        #endregion

        #region - Validation Parameters

        protected EditContext? EditContext;

        protected ValidationMessageStore? ValidationMessageStore;

        protected bool HasValidationError = false;

        #endregion

        #region - Calendar Culture -

        protected CultureInfo GetCalendarCulture(string Culture)
        {
            switch (Culture)
            {
                case "fa-IR":
                    return GetPersianCulture(Culture);
                default:
                    return new CultureInfo(Culture);
            }
        }

        private CultureInfo GetPersianCulture(string Culture)
        {
            CultureInfo cultureInfo = new CultureInfo(Culture);

            DateTimeFormatInfo formatInfo = cultureInfo.DateTimeFormat;
            formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
            formatInfo.DayNames = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهار شنبه", "پنجشنبه", "جمعه", "شنبه" };
            var monthNames = new[]
            {
            "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن",
            "اسفند",
            "",
        };
            formatInfo.AbbreviatedMonthNames =
                formatInfo.MonthNames =
                    formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
            formatInfo.AMDesignator = "ق.ظ";
            formatInfo.PMDesignator = "ب.ظ";
            formatInfo.ShortDatePattern = "yyyy/MM/dd";
            formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";
            formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;
            Calendar cal = new PersianCalendar();
            
            var calendarField = cultureInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
            if (calendarField != null)
                calendarField.SetValue(cultureInfo, cal);
           
            var formatCalendarField = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
            if (formatCalendarField != null)
                formatCalendarField.SetValue(formatInfo, cal);

            cultureInfo.NumberFormat.NumberDecimalSeparator = "/";
            cultureInfo.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
            cultureInfo.NumberFormat.NumberNegativePattern = 0;

            return cultureInfo;
        }

        #endregion

    }
}
