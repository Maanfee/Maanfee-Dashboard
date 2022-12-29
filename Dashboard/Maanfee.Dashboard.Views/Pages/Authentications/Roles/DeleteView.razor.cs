using Maanfee.Dashboard.Core;
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
    public partial class DeleteView
    {
        private IdentityRole IdentityRole = new();

        [Parameter] 
        public string IdRole { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await PermissionService.CheckAuthorizeAsync(PermissionDefaultValue.Role.Delete, PermissionAuthenticationState,
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

        private async Task OnDelete()
        {
            if (IsProcessing)
                return;
            IsProcessing = true;

            try
            {
                var DeleteResult = await Http.DeleteAsync($"api/Roles/Delete/{IdRole}");
                if (DeleteResult.IsSuccessStatusCode)
                {
                    var JsonResult = await DeleteResult.Content.ReadFromJsonAsync<CallbackResult<IdentityRole>>();
                    if (JsonResult.Data != null)
                    {
                        Navigation.NavigateTo("/Roles/IndexView");
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
