using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Base.Services;
using Maanfee.Dashboard.Views.Core;
using Maanfee.Dashboard.Views.Core.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Base.Pages
{
    public class _BaseView : _BaseComponentView
    {
        // *****************************************

        [CascadingParameter]
        protected Task<AuthenticationState> PermissionAuthenticationState { get; set; }

        //protected ClaimsPrincipal PermissionCurrentUser;

        //protected bool ViewPermissions = false;

        [Inject]
        protected PermissionService PermissionService { get; set; }

        // *****************************************

        protected async Task FormValidationCallback(string Message)
        {
            if (!string.IsNullOrEmpty(Message))
            {
                Snackbar.Add(Message, Severity.Warning);
            }

            IsProcessing = false;

            await Task.CompletedTask;
        }

        // *****************************************

        protected CultureInfo GetPersianCulture()
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

        // *****************************************

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var ModuleList = await Http.GetFromJsonAsync<List<ModuleViewModel>>("config.json");
                //ModuleService.Automation = ModuleList.FirstOrDefault(x => x.Name == ModuleDefaultValue.Automation);
                //ModuleService.RollCall = ModuleList.FirstOrDefault(x => x.Name == ModuleDefaultValue.RollCall);
                //ModuleService.Attendance = ModuleList.FirstOrDefault(x => x.Name == ModuleDefaultValue.Attendance);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }

			//var Model = new JwtLoginViewModel
			//{
			//	UserName = "Maanfee", // loginRequest.UserName,
			//	Password = "Maanfee", // loginRequest.Password,
			//};

			#region - Automation -

			//try
			//{
			//	if (ModuleService.Automation.IsActive)
			//	{
			//		var JwtTokenStorage = ModuleService.Automation.Name;

			//		var PostJwt = await Http.PostAsJsonAsync($"{ModuleService.Automation.ToExactUri(Http)}/accounts/login", Model);
			//		if (PostJwt.IsSuccessStatusCode)
			//		{
			//			var JsonResult = await PostJwt.Content.ReadFromJsonAsync<JwtAuthenticationViewModel>();
			//			if (JsonResult != null)
			//			{
			//				await LocalStorage.SetAsync(JwtTokenStorage, JsonResult.Token);
			//				((JwtAuthenticationStateProvider)JwtAuthenticationStateProvider).JwtTokenStorage = JwtTokenStorage;
			//				((JwtAuthenticationStateProvider)JwtAuthenticationStateProvider).NotifyUserAuthentication(Model.UserName);
			//				Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", JsonResult.Token);

			//				ModuleService.Automation.CanNavigation = true;
			//			}
			//			else
			//			{
			//				Snackbar.Add(JsonResult.ErrorMessage, Severity.Error);
			//			}
			//		}
			//		else
			//		{
			//			Snackbar.Add(PostJwt.Content.ReadAsStringAsync().Result, Severity.Error);
			//		}
			//	}
			//}
			//catch
			//{
			//	ModuleService.Automation.CanNavigation = false;
			//	Snackbar.Add($"{DashboardResource.StringError} : {DashboardResource.MessageServiceCommunicationError}", Severity.Error);
			//}

			#endregion
		}

	}
}
