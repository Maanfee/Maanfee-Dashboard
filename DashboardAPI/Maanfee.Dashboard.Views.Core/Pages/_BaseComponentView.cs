using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Views.Core.Services;
using Maanfee.Web.Core;
using Maanfee.Web.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Globalization;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Core
{
    public class _BaseComponentView : ComponentBase, IDisposable
    {
#pragma warning disable CS0108

        [Inject]
        protected HttpClient Http { get; set; }

		[Inject]
        protected NavigationManager Navigation { get; set; }

        //[Inject]
        //protected CustomStateProvider AuthenticationStateProvider { get; set; }

        //[Inject]
        //protected IJSRuntime JS { get; set; }

        [Inject]
        protected AccountStateContainer AccountStateContainer { get; set; }

        // JWT
        [Inject]
        protected LocalConfigurationService LocalConfiguration { get; set; }

        // JWT
        [Inject]
        protected LocalStorage LocalStorage { get; set; }

        // JWT
        [Inject]
        protected JwtAuthenticationStateProvider JwtAuthenticationStateProvider { get; set; }

        [Inject]
        protected IDialogService Dialog { get; set; }

		[Inject]
		protected Fullscreen Fullscreen { get; set; }

		[Inject]
		protected TableConfigurationService TableConfiguration { get; set; }

#pragma warning restore CS0108

		// *****************************************

		protected bool IsProcessing = false;

        protected bool IsLoaded = false;

        protected bool IsTableLoading = true;

        // *****************************************

        protected override void OnInitialized() 
        {
            AccountStateContainer.OnChange += StateHasChanged;
			Fullscreen.OnFullscreenChange += OnFullscreenChange;
		}

		public void Dispose()
        {
            AccountStateContainer.OnChange -= StateHasChanged;
			Fullscreen.OnFullscreenChange -= OnFullscreenChange;
		}

		// *****************************************

		#region - MudTable Config -

		protected bool _IsTableDense { get; set; } = true;

        protected bool _IsTableFixedHeader { get; set; } = true;
        
		protected bool _IsTableScroll { get; set; } = true;
		
		protected static string TableHeight { get; set; } = "320px";

		private async void OnFullscreenChange()
		{
			await Task.Delay(500);

			TableHeight = TableConfiguration.SetHeight(SharedLayoutSettings.IsRTL, await Fullscreen.IsFullscreenAsync(), _IsTableScroll);

			StateHasChanged();
		}

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
					return GetPersianCulture();
				default:
					return new CultureInfo(Culture);
			}
		}

		private CultureInfo GetPersianCulture()
		{
			var culture = new CultureInfo("fa-IR");
			DateTimeFormatInfo formatInfo = culture.DateTimeFormat;
			formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
			formatInfo.DayNames = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهار شنبه", "پنجشنبه", "جمعه", "شنبه" };
			var monthNames = new[]
			{
				"فروردین", "اردیبهشت", "خرداد",
				"تیر", "مرداد", "شهریور",
				"مهر", "آبان", "آذر",
				"دی", "بهمن", "اسفند",
				"",
			};
			formatInfo.AbbreviatedMonthNames = monthNames;
			formatInfo.MonthNames =
					formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
			formatInfo.AMDesignator = "ق.ظ";
			formatInfo.PMDesignator = "ب.ظ";
			formatInfo.ShortDatePattern = "yyyy/MM/dd";
			formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";
			formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;
			Calendar cal = new PersianCalendar();
			FieldInfo fieldInfo = culture.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
			if (fieldInfo != null)
				fieldInfo.SetValue(culture, cal);
			FieldInfo info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
			if (info != null)
				info.SetValue(formatInfo, cal);
			culture.NumberFormat.NumberDecimalSeparator = "/";
			culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
			culture.NumberFormat.NumberNegativePattern = 0;
			return culture;
		}

		#endregion

	}
}
