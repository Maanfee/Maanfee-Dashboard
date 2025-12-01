using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Views.Core.DefaultValues;
using Maanfee.Dashboard.Views.Core.Services;
using Maanfee.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using MudBlazor.Utilities;
using System.Globalization;
using System.Reflection;

namespace Maanfee.Dashboard.Views.Core
{
    public class _BaseComponentView : LayoutComponentBase, IDisposable
    {
#pragma warning disable CS0108

        [Inject]
        protected HttpClient Http { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        //[Inject]
        //protected CustomStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        protected AccountStateContainer AccountStateContainer { get; set; }

        // JWT
        [Inject]
        protected LocalConfigurationService LocalConfiguration { get; set; }

        // JWT
        [Inject]
        protected Maanfee.JsInterop.LocalStorage LocalStorage { get; set; }

        // JWT
        [Inject]
        protected JwtAuthenticationStateProvider JwtAuthenticationStateProvider { get; set; }

        [Inject]
        protected IDialogService Dialog { get; set; }

        [Inject]
        protected Fullscreen Fullscreen { get; set; }

        //[Inject]
        //protected Screen Screen { get; set; }

        [Inject]
        protected TableConfigurationService TableConfiguration { get; set; }

#pragma warning restore CS0108

        // *****************************************

        protected bool IsProcessing = false;

        protected bool IsLoaded = false;

        protected bool IsTableLoading = true;

        // *****************************************

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            AccountStateContainer.OnChange += StateHasChanged;

            await SaveDefaultsAsync();
        }

        public void Dispose()
        {
            AccountStateContainer.OnChange -= StateHasChanged;
        }

        // *****************************************

        #region - Save Defaults -

        protected LanguageModel LanguageModel = new();

        protected virtual MudTheme CurrentTheme { get; set; }

        protected async Task SaveDefaultsAsync()
        {
            LanguageModel = await LocalStorage.GetAsync<LanguageModel>(StorageDefaultValue.CultureStorage);
            SharedLayoutSettings.IsRTL = LanguageModel.IsRTL;

            CurrentTheme = MaanfeeTheme.ThemeBuilder(SharedLayoutSettings.IsRTL, SharedLayoutSettings.IsDarkMode, (MudColor)SharedLayoutSettings.ThemeColor);
        }

        #endregion

        #region - MudTable Config -

        protected bool _IsTableDense { get; set; } = true;

        protected bool _IsTableFixedHeader { get; set; } = true;

        protected bool _IsTableScroll { get; set; } = true;

        public static string TableHeight { get; set; } = TableConfigurationService.InitTableHeight;

        public static string TableHeightLarge { get; set; } = TableConfigurationService.InitTableHeightLarge;

        protected async void OnScrollChanged()
        {
            _IsTableScroll = _IsTableScroll ? false : true;

            TableHeight = TableConfiguration.SetHeight(SharedLayoutSettings.IsRTL, await Fullscreen.IsFullscreenAsync(), _IsTableScroll);

            StateHasChanged();
        }

        #endregion

        #region - Snackbar Config -

        [Inject]
        protected ISnackbar Snackbar { get; set; }

        #endregion

        #region - Validation Parameters

        protected EditContext EditContext;

        protected ValidationMessageStore ValidationMessageStore;

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
            System.Globalization.Calendar cal = new PersianCalendar();
            FieldInfo fieldInfo = cultureInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
                fieldInfo.SetValue(cultureInfo, cal);
            FieldInfo info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
            if (info != null)
                info.SetValue(formatInfo, cal);
            cultureInfo.NumberFormat.NumberDecimalSeparator = "/";
            cultureInfo.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
            cultureInfo.NumberFormat.NumberNegativePattern = 0;

            return cultureInfo;
        }

        #endregion

    }
}
