using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using static MudBlazor.CategoryTypes;

namespace Maanfee.Dashboard.Views.Pages.Authentications.Roles
{
    public partial class DialogPermission
    {
        [Parameter]
        public string IdRole { get; set; }

        [Parameter]
        public string RoleName { get; set; }

        public const string PermissionClaimValue = "Permission";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            try
            {
                SelectedValues = null;

                await GetRoleClaimViewModelsAsync();
                await GetPermissionsAsync();
                await BuildTreeAsync();

                SelectedValues = Permissions.Where(i => GetRoleClaimFromDatabase.Any(a => i.FullName == a.ClaimType)).ToList();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }
            finally
            {
                IsLoaded = true;
            }
        }

        private List<Permission> Permissions = new();

        private async Task GetPermissionsAsync()
        {
            var Callback = await Http.GetFromJsonAsync<CallbackResult<List<Permission>>>($"api/Permissions/GetPermissions");
            if (Callback.Data != null)
            {
                Permissions = Callback.Data;
            }
            else
            {
                Snackbar.Add(Callback.Error.ToString(), Severity.Error);
            }
        }

        public IReadOnlyCollection<Permission> SelectedValues { get; set; }

        #region - Tree Builder -

        private List<TreeItemData<Permission>> TreeItems { get; set; } = new();

        private async Task BuildTreeAsync()
        {
            TreeItems = await BuildTreeItems(null);
        }

        private Task<List<TreeItemData<Permission>>> BuildTreeItems(string parentId)
        {
            var items = new List<TreeItemData<Permission>>();

            var departments = Permissions.Where(d => d.IdParent == parentId).ToList();

            foreach (var dept in departments)
            {
                var treeItem = new TreeItemData<Permission>
                {
                    Value = dept,
                    Text = $"{dept.Title} ({dept.DisplayTitle})",
                    Expandable = Permissions.Any(child => child.IdParent == dept.Id),
                    Icon = Icons.Material.Filled.Groups,
                    Children = new List<TreeItemData<Permission>>()
                };

                // به صورت بازگشتی فرزندان را هم اضافه کن
                if (treeItem.Expandable)
                {
                    treeItem.Children = BuildTreeItems(dept.Id).Result;
                }

                items.Add(treeItem);
            }

            return Task.FromResult(items);
        }

        public async Task<IReadOnlyCollection<TreeItemData<Permission>>> LoadServerData(Permission parentValue)
        {
            try
            {
                if (parentValue == null)
                {
                    // بارگذاری ریشه‌ها
                    var roots = Permissions.Where(d => d.IdParent == null).ToList();
                    return roots.Select(d => new TreeItemData<Permission>
                    {
                        Value = d,
                        Text = $"{d.Title} ({d.DisplayTitle})",
                        Expandable = Permissions.Any(child => child.IdParent == d.Id),
                        Children = new List<TreeItemData<Permission>>()
                    }).ToList().AsReadOnly();
                }
                else
                {
                    // بارگذاری فرزندان یک والد خاص
                    var children = Permissions.Where(d => d.IdParent == parentValue.Id).ToList();
                    return children.Select(d => new TreeItemData<Permission>
                    {
                        Value = d,
                        Text = $"{d.Title} ({d.DisplayTitle})",
                        Expandable = Permissions.Any(child => child.IdParent == d.Id),
                        Children = new List<TreeItemData<Permission>>()
                    }).ToList().AsReadOnly();
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{ex.Message}", Severity.Error);
                return new List<TreeItemData<Permission>>().AsReadOnly();
            }
        }

        #endregion

        // *******************************

        private List<GetRoleClaimViewModel> GetRoleClaimFromDatabase = [];

        private async Task GetRoleClaimViewModelsAsync()
        {
            try
            {
                var Callback = await Http.GetFromJsonAsync<CallbackResult<List<GetRoleClaimViewModel>>>($"api/RoleClaim/GetRoleClaims?IdRole={IdRole}");
                if (Callback.Data != null)
                {
                    GetRoleClaimFromDatabase = Callback.Data;
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

        // **********************************************

        private List<SubmitRoleClaimViewModel> SubmitModels { get; set; } = [];

        private async Task SaveAsync()
        {
            if (IsProcessing)
                return;
            IsProcessing = true;

            try
            {
                if (SelectedValues.Any())
                {
                    foreach (var item in SelectedValues)
                    {
                        var RoleClaim = new SubmitRoleClaimViewModel();
                        RoleClaim.ClaimType = item.FullName;
                        RoleClaim.ClaimValue = PermissionClaimValue;
                        RoleClaim.RoleId = IdRole;
                        RoleClaim.IsSelected = true;

                        SubmitModels.Add(RoleClaim);
                    }
                }
                else
                {
                    // در صورتیکه قبلا ایتمی وجود داشته و حالا حذف شده کل درخت را پاک می کند
                    var RoleClaim = new SubmitRoleClaimViewModel();
                    RoleClaim.ClaimType = string.Empty;
                    RoleClaim.ClaimValue = PermissionClaimValue;
                    RoleClaim.RoleId = IdRole;
                    RoleClaim.IsSelected = false;

                    SubmitModels.Add(RoleClaim);
                }

                var PostResult = await Http.PostAsJsonAsync("api/RoleClaim/CreateRange", SubmitModels);
                if (PostResult.IsSuccessStatusCode)
                {
                    var JsonResult = await PostResult.Content.ReadFromJsonAsync<CallbackResult<IList<SubmitRoleClaimViewModel>>>();
                    if (JsonResult?.Data != null)
                    {
                        await GetRoleClaimViewModelsAsync();
                        Snackbar.Add(JsonResult.SuccessMessage ?? DashboardResource.MessageSavedSuccessfully, Severity.Success);
                        MDialog.Close();
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
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }
            finally
            {
                IsProcessing = false;
                await PermissionStateContainer?.LoadPermissionsAsync(Http, AccountStateContainer.Id);
                StateHasChanged();
            }
        }

    }
}
