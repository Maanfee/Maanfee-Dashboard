using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Base;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Pages.Settings.SettingsConfig
{
    public partial class SettingsView
    {
        protected override async Task OnInitializedAsync()
        {
            IsLoaded = true;

            try
            {
                await PermissionService.CheckAuthorizeAsync(PermissionDefaultValue.Setting.SettingsView, AuthenticationState,
                     AuthorizationService, Navigation);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }
        }
    }
}
