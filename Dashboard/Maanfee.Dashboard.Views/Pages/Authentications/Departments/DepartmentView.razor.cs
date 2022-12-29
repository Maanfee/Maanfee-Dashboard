using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Base;
using Maanfee.Web.Core;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Pages.Authentications.Departments
{
    public partial class DepartmentView
    {
        private SubmitDepartmentViewModel SubmitDepartmentViewModel = new();

        //private Department ActivatedValue = new(); 

        private Department SelectedValue = new();
        //private HashSet<Department> SelectedValues { get; set; }

        private bool CanCreate = false;

        private bool CanDelete = false;

        protected override async Task OnInitializedAsync()
        {
            IsLoaded = false;

            try
            {
                await PermissionService.CheckAuthorizeAsync(PermissionDefaultValue.Department.View, PermissionAuthenticationState,
                     AuthorizationService, Navigation);

                CanCreate = await PermissionService.IsAuthorizeAsync(PermissionDefaultValue.Department.Create, PermissionAuthenticationState,
                    AuthorizationService, Navigation);

                CanDelete = await PermissionService.IsAuthorizeAsync(PermissionDefaultValue.Department.Delete, PermissionAuthenticationState,
                      AuthorizationService, Navigation);

                await ResetAsync();

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
                var PostResult = await Http.PostAsJsonAsync("api/Departments/CreateOrUpdate", SubmitDepartmentViewModel.TrimString());
                if (PostResult.IsSuccessStatusCode)
                {
                    var JsonResult = await PostResult.Content.ReadFromJsonAsync<CallbackResult<Department>>();
                    if (JsonResult?.Data != null)
                    {
                        Snackbar.Add(JsonResult.SuccessMessage ?? DashboardResource.MessageSavedSuccessfully, Severity.Success);

                        await ResetAsync();
                    }
                    else
                    {
                        // ======== فقط برای این فرم ========
                        if (JsonResult.Error.Message.Contains("This item could not be a subset of itself"))
                        {
                            Snackbar.Add(DashboardResource.MessageThisItemCouldNoBeASubsetOfItself, Severity.Error);
                        }
                        else
                        {
                            Snackbar.Add(MessageHandler.ErrorHandler(JsonResult.Error), Severity.Error);
                        }
                        // ==================================
                    }
                }
                else
                {
                    Snackbar.Add(PostResult.Content.ReadAsStringAsync().Result, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }
            IsProcessing = false;
        }

        #region - Combo & Dropdown -

        private HashSet<Department> Departments = new();

        private List<DropDownDepartmentViewModel> Parents = new();

        private async Task<IEnumerable<DropDownDepartmentViewModel>> GetParentsAsync(string value)
        {
            var Callback = await Http.GetFromJsonAsync<CallbackResult<List<DropDownDepartmentViewModel>>>($"api/Departments/GetDropDownDepartments?value={value}");
            if (Callback.Data != null)
            {
                Parents = Callback.Data.Select(x => new DropDownDepartmentViewModel
                {
                    Id = x.Id,
                    IdParent = x.IdParent,
                    Title = x.Title,
                }).ToList();
            }
            else
            {
                Snackbar.Add(Callback.Error.ToString(), Severity.Error);
            }
            return Parents;
        }

        private async Task GetDepartmentsAsync()
        {
            var Callback = await Http.GetFromJsonAsync<CallbackResult<HashSet<Department>>>($"api/Departments/Index");
            if (Callback.Data != null)
            {
                Departments = Callback.Data;
            }
            else
            {
                Snackbar.Add(Callback.Error.ToString(), Severity.Error);
            }
        }

        #endregion

        private async Task ResetAsync()
        {
            await GetDepartmentsAsync();
            await GetParentsAsync(string.Empty);

            SubmitDepartmentViewModel.Id = null;
            SubmitDepartmentViewModel.Title = string.Empty;
            SubmitDepartmentViewModel.Parent = null;
        }

        private async Task CreateAsync()
        {
            await GetDepartmentsAsync();
            await GetParentsAsync(string.Empty);

            SubmitDepartmentViewModel.Id = null;
            SubmitDepartmentViewModel.Title = string.Empty;
        }

        #region - Dialog -

        private DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true };
        private DialogParameters parameters = new DialogParameters();

        private async Task OpenDeleteDialog(DialogOptions options)
        {
            if (SubmitDepartmentViewModel.Id == 0)
            {
                parameters.Add("Title", DashboardResource.StringPageNotExist);
            }
            else
            {
                parameters.Add("Title", SubmitDepartmentViewModel.Title);
            }

            var dialog = Dialog.Show<DialogDelete>(DashboardResource.StringAlert, parameters, options);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                try
                {
                    var DeleteResult = await Http.DeleteAsync($"api/Departments/Delete/{SubmitDepartmentViewModel.Id}");
                    if (DeleteResult.IsSuccessStatusCode)
                    {
                        var JsonResult = await DeleteResult.Content.ReadFromJsonAsync<CallbackResult<Department>>();
                        if (JsonResult.Data != null)
                        {
                            Snackbar.Add(JsonResult.SuccessMessage ?? DashboardResource.MessageDeletedSuccessfully, Severity.Success);

                            await ResetAsync();
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
            }
        }

        private void OpenDepartmentPermissionDialog()
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add("IdDepartment", SubmitDepartmentViewModel?.Id);

            //Dialog.Show<DialogDepartmentPermit>(String.Empty, parameters, new DialogOptions()
            //{
            //    MaxWidth = MaxWidth.ExtraExtraLarge,
            //    FullWidth = true,
            //    Position = DialogPosition.TopCenter,
            //});
        }

        #endregion

    }
}
