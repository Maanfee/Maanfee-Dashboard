using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Base;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Pages.Authentications.Roles
{
    public partial class DetailsView
    {
        private IdentityRole IdentityRole = new();

        [Parameter] 
        public string IdRole { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await PermissionService.CheckAuthorizeAsync(PermissionDefaultValue.Role.Details, PermissionAuthenticationState,
                  AuthorizationService, Navigation);

                var Callback = await Http.GetFromJsonAsync<CallbackResult<IdentityRole>>($"api/Roles/Details/{IdRole}");

                if (Callback.Data != null)
                {
                    IdentityRole = Callback.Data;
                }
                else
                {
                    Snackbar.Add(Callback.Error.ToString(), Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }
        }
    
    }
}
