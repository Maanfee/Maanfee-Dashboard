using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.ViewModels;
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
    public partial class CrudateView
    {
        [Parameter]
        public string Id { get; set; }

        private SubmitRoleViewModel SubmitRoleViewModel = new();

        protected async override Task OnInitializedAsync()
        {
            IsLoaded = false;

            try
            {
                if (!string.IsNullOrEmpty(Id) && Id != "0")
                {
                    await PermissionService.CheckAuthorizeAsync(PermissionDefaultValue.Role.Edit, PermissionAuthenticationState,
                   AuthorizationService, Navigation);

                    var Callback = await Http.GetFromJsonAsync<CallbackResult<IdentityRole>>($"api/Roles/Details/{Id}");
                    if (Callback.Data != null)
                    {
                        SubmitRoleViewModel.Id = Callback.Data.Id;
                        SubmitRoleViewModel.Role = Callback.Data.Name;
                    }
                    else
                    {
                        Snackbar.Add(Callback.Error.ToString(), Severity.Error);
                    }
                }
                else
                {
                    await PermissionService.CheckAuthorizeAsync(PermissionDefaultValue.Role.Create, PermissionAuthenticationState,
                        AuthorizationService, Navigation);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }

            IsLoaded = true;
        }

        private async Task OnSubmit()
        {
            if (IsProcessing)
                return;
            IsProcessing = true;
                      
            try
            {
                if (Id == "0")
                {
                    var PostResult = await Http.PostAsJsonAsync("api/Roles/Create", SubmitRoleViewModel.TrimString());
                    if (PostResult.IsSuccessStatusCode)
                    {
                        var JsonResult = await PostResult.Content.ReadFromJsonAsync<CallbackResult<SubmitRoleViewModel>>();
                        if (JsonResult?.Data != null)
                        {
                            Navigation.NavigateTo("/Roles/IndexView");
                            Snackbar.Add(JsonResult.SuccessMessage ?? DashboardResource.MessageSavedSuccessfully, Severity.Success);
                        }
                        else
                        {
                            Snackbar.Add(MessageHandler.ErrorHandler(JsonResult.Error), Severity.Error);
                        }
                    }
                    else
                    {
                        Snackbar.Add(PostResult.Content.ReadAsStringAsync().Result, Severity.Error);
                    }
                }
                else
                {
                    var PutResult = await Http.PutAsJsonAsync("api/Roles/Edit", SubmitRoleViewModel.TrimString());
                    if (PutResult.IsSuccessStatusCode)
                    {
                        var JsonResult = await PutResult.Content.ReadFromJsonAsync<CallbackResult<SubmitRoleViewModel>>();
                        if (JsonResult.Data != null)
                        {
                            Navigation.NavigateTo("/Roles/IndexView");
                            Snackbar.Add(JsonResult.SuccessMessage ?? DashboardResource.MessageSavedSuccessfully, Severity.Success);
                        }
                        else
                        {
                            Snackbar.Add(MessageHandler.ErrorHandler(JsonResult.Error), Severity.Error);
                        }
                    }
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