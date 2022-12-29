using Maanfee.Dashboard.Core;
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
    public partial class DeleteView
    {
        [Parameter]
        public string IdUser { get; set; }

        private GetUserViewModel ApplicationUser = new();

        private string StringAvatar;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await PermissionService.CheckAuthorizeAsync(PermissionDefaultValue.User.Delete, PermissionAuthenticationState,
                     AuthorizationService, Navigation);

                var Callback = await Http.GetFromJsonAsync<CallbackResult<GetUserViewModel>>($"api/Users/GetUserById/{IdUser}");

                if (Callback.Data != null)
                {
                    ApplicationUser = Callback.Data;

                    StringAvatar = ("data:image/png;base64," + Convert.ToBase64String(ApplicationUser.Avatar));
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

        private async Task OnDelete()
        {
            if (IsProcessing)
                return;
            IsProcessing = true;

            try
            {
                var DeleteResult = await Http.DeleteAsync($"api/Authentications/Delete/{IdUser}");
                if (DeleteResult.IsSuccessStatusCode)
                {
                    var JsonResult = await DeleteResult.Content.ReadFromJsonAsync<CallbackResult<GetUserViewModel>>();
                    if (JsonResult.Data != null)
                    {
                        Navigation.NavigateTo("/Users/IndexView");
                        Snackbar.Add(JsonResult.SuccessMessage ?? DashboardResource.MessageDeletedSuccessfully, Severity.Success);
                    }
                    else
                    {
                        Snackbar.Add(MessageHandler.ErrorHandler(JsonResult.Error), Severity.Error);
                    }
                }
                else
                {
                    Snackbar.Add(DeleteResult.Content.ReadAsStringAsync().Result, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }
            IsProcessing = false;
        }
  
    }
}
