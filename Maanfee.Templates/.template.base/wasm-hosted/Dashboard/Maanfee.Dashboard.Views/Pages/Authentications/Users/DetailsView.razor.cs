using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Base;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Pages.Authentications.Users
{
    public partial class DetailsView
    {
        [Parameter]
        public string IdUser { get; set; }

        private GetUserViewModel ApplicationUser = new();

        private string Avatar;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await PermissionService.CheckAuthorizeAsync(PermissionDefaultValue.User.Details, PermissionAuthenticationState,
                    AuthorizationService, Navigation);

                var Callback = await Http.GetFromJsonAsync<CallbackResult<GetUserViewModel>>($"api/Users/GetUserById/{IdUser}");

                if (Callback.Data != null)
                {
                    ApplicationUser = Callback.Data;

                    Avatar = ("data:image/png;base64," + Convert.ToBase64String(ApplicationUser.Avatar));
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
