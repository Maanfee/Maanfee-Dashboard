using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Base;
using Microsoft.AspNetCore.Authorization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Pages.Settings
{
    public partial class SettingsView2
    {
        protected override async Task OnInitializedAsync()
        {
            IsLoaded = true;

            try
            {
                //await PermissionService.CheckAuthorizeAsync(PermissionDefaultValue.Setting.View, PermissionAuthenticationState,
                //     _authorizationService, Navigation);
                test();

                await Task.Delay(100); 
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }
        }

        private List<string> val = new();

        public void test()
        {
            foreach (var prop in typeof(PermissionDefaultValue).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                {
                    val.Add(propertyValue.ToString());
                }
            }
        }

    }
}
