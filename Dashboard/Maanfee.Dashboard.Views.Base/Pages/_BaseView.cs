using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Base.Services;
using Maanfee.Dashboard.Views.Core;
using Maanfee.Dashboard.Views.Core.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Base.Pages
{
    public class _BaseView : _BaseComponentView
    {
        [Inject]
        protected GatewayApi GatewayApi { get; set; }

        // *****************************************

        #region - Authentication -

        [CascadingParameter]
        protected Task<AuthenticationState> AuthenticationState { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        protected ClaimsPrincipal PermissionCurrentUser { get; set; }

        //protected bool ViewPermissions = false;

        [Inject]
        protected PermissionService PermissionService { get; set; }

        #endregion

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
            await base.OnInitializedAsync();

            try
            {
                AuthenticationState = Task.FromResult(await AuthenticationStateProvider.GetAuthenticationStateAsync());
                PermissionCurrentUser = (await AuthenticationState).User;

                var ModuleList = await Http.GetFromJsonAsync<List<ModuleViewModel>>("config.json");
                //ModuleService.LogServer = ModuleList.FirstOrDefault(x => x.Name == ModuleDefaultValue.LogServer);
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

            //try
            //{
            //    if (ModuleService.LogServer.IsActive)
            //    {
            //        var JwtTokenStorage = ModuleService.LogServer.Name;

            //        var PostResult = await Http.PostAsJsonAsync($"{GatewayApi.ToUri}/Accounts/Login", Model.TrimStringAndCheckPersianSpecialLetter());
            //        if (PostResult.IsSuccessStatusCode)
            //        {
            //            var JsonResult = await PostResult.Content.ReadFromJsonAsync<JwtAuthenticationViewModel>();
            //            if (JsonResult != null)
            //            {
            //                await LocalStorage.SetAsync(JwtTokenStorage, JsonResult.Token);
            //                ((JwtAuthenticationStateProvider)JwtAuthenticationStateProvider).JwtTokenStorage = JwtTokenStorage;
            //                ((JwtAuthenticationStateProvider)JwtAuthenticationStateProvider).NotifyUserAuthentication(Model.UserName);
            //                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", JsonResult.Token);

            //                ModuleService.LogServer.CanNavigation = true;

            //                //Snackbar.Add(JsonResult.Token, Severity.Error);
            //            }
            //            else
            //            {
            //                Snackbar.Add(PostResult.Content.ReadAsStringAsync().Result, Severity.Error);
            //            }
            //        }
            //        else
            //        {
            //            Snackbar.Add(PostResult.Content.ReadAsStringAsync().Result, Severity.Error);
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    ModuleService.LogServer.CanNavigation = false;
            //    Snackbar.Add($"{DashboardResource.StringError} : {DashboardResource.MessageServiceCommunicationError} - {ex.InnerException.Message}", Severity.Error);
            //}

            #endregion
        }

    }
}
