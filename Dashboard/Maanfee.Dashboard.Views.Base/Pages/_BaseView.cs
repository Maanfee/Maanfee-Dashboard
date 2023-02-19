using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Base.Services;
using Maanfee.Dashboard.Views.Core;
using Maanfee.Dashboard.Views.Core.Services;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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

		protected override async Task OnInitializedAsync()
		{
			try
			{
				var ModuleList = await Http.GetFromJsonAsync<List<ModuleViewModel>>("config.json");
				ModuleService.LogServer = ModuleList.FirstOrDefault(x => x.Name == ModuleDefaultValue.LogServer);
			}
			catch (Exception ex)
			{
				Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
			}

			var Model = new JwtLoginViewModel
			{
				UserName = "Maanfee", // loginRequest.UserName,
				Password = "Maanfee", // loginRequest.Password,
			};

			#region - Log Server -

			try
			{
				if (ModuleService.LogServer.IsActive)
				{
					var JwtTokenStorage = ModuleService.LogServer.Name;

					var ApiResult = await ApiGatewayClient.PostAsJsonAsync<JwtLoginViewModel, JwtAuthenticationViewModel>("http://localhost:4030/gateway/Accounts/Login", Model.TrimString());

					if (ApiResult.Data != null)
					{
						await LocalStorage.SetAsync(JwtTokenStorage, ApiResult.Data.Token);
						((JwtAuthenticationStateProvider)JwtAuthenticationStateProvider).JwtTokenStorage = JwtTokenStorage;
						((JwtAuthenticationStateProvider)JwtAuthenticationStateProvider).NotifyUserAuthentication(Model.UserName);
						Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", ApiResult.Data.Token);

						ModuleService.LogServer.CanNavigation = true;
					}
					else
					{
						Snackbar.Add($"{DashboardResource.StringError} : " + ApiResult.Error.Message, Severity.Error);
					}

					//var Callback = await Http.GetStringAsync($"http://localhost:4030/Gateway/Test/ConnectionTest");
					//if (!string.IsNullOrEmpty(Callback))
					//{
					//	Snackbar.Add(Callback, Severity.Info);
					//}
					//else
					//{
					//	Snackbar.Add("KOOOOOOOOOOOOO", Severity.Error);
					//}
					//Snackbar.Add(ApiResult.Data.Token, Severity.Error);
				}
			}
			catch (Exception ex)
			{
				ModuleService.LogServer.CanNavigation = false;
				Snackbar.Add($"{DashboardResource.StringError} : {DashboardResource.MessageServiceCommunicationError} - {ex.InnerException.Message}", Severity.Error);
			}

			#endregion
		}

	}
}
