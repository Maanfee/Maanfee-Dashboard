using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Core;
using Maanfee.Web.Core;
using MudBlazor;
using System.Net.Http.Json;

namespace Maanfee.Dashboard.Views.Pages.Authentications.Permissions
{
    public partial class PermissionView
    {
        private SubmitPermissionViewModel Model = new();

        //private Permission ActivatedValue = new(); 

        private Permission SelectedValue = new();
        //private HashSet<Permission> SelectedValues { get; set; }

        public const string View = "Permission.Dashboard.Permissions";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            IsLoaded = false;

            try
            {
                PermissionStateContainer.HasPermissionToDisplayView(View, Navigation);

                SelectedValue = null;

                await ResetAsync();
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

        #region - Tree Builder -

        //private MudTreeView<Department> TreeView;

        private List<TreeItemData<Permission>> TreeItems { get; set; } = new();

        private async Task BuildTreeAsync()
        {
            TreeItems = await BuildTreeItems(null);
        }

        private Task<List<TreeItemData<Permission>>> BuildTreeItems(string parentId)
        {
            var items = new List<TreeItemData<Permission>>();

            var permissions = Permissions.Where(d => d.IdParent == parentId).ToList();

            foreach (var dept in permissions)
            {
                var treeItem = new TreeItemData<Permission>
                {
                    Value = dept,
                    Text = dept.Title,
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
                        Text = d.Title,
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
                        Text = d.Title,
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

        #region - Search -

        //private string SearchPhrase;

        //private async void OnTextChanged(string searchPhrase)
        //{
        //    SearchPhrase = searchPhrase;
        //    Permissions = Permissions.Where(d => d.Title.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase)).ToList();
        //    await BuildTreeAsync();
        //}

        #endregion

        private async Task OnSubmit()
        {
            if (IsProcessing)
                return;
            IsProcessing = true;

            Model.FullName = $"Permission.{Model?.Parent?.Name}.{Model.Title}".Replace("..",".");

            try
            {
                var PostResult = await Http.PostAsJsonAsync("api/Permissions/CreateOrUpdate", Model.TrimStringAndCheckPersianSpecialLetter());
                if (PostResult.IsSuccessStatusCode)
                {
                    var JsonResult = await PostResult.Content.ReadFromJsonAsync<CallbackResult<Permission>>();
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
            finally
            {
                IsProcessing = false;
            }
        }

        #region - Combo & Dropdown -

        private List<DropDownPermissionViewModel> Parents = new();

        private async Task<IEnumerable<DropDownPermissionViewModel>> GetParentsAsync(string value, CancellationToken token)
        {
            var Callback = await Http.GetFromJsonAsync<CallbackResult<List<DropDownPermissionViewModel>>>($"api/Permissions/GetDropDownPermissions?value={value}&", token);
            if (Callback.Data != null)
            {
                Parents = Callback.Data.ToList();
            }
            else
            {
                Snackbar.Add(Callback.Error.ToString(), Severity.Error);
            }
            return Parents;
        }

        #endregion

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

        private async Task ResetAsync()
        {
            await GetPermissionsAsync();
            await BuildTreeAsync();
            await GetParentsAsync(string.Empty, new CancellationTokenSource().Token);

            Model.Id = null;
            Model.Title = string.Empty;
            Model.DisplayTitle = string.Empty;
            Model.FullName = string.Empty;
            Model.Parent = null;
        }

		#region - Delete Dialog -

		private async Task OpenDeleteDialog()
        {
            DialogParameters parameters = new DialogParameters();

            var dialog = await Dialog.ShowAsync<DialogDelete>(DashboardResource.StringAlert, parameters,
                new DialogOptions()
                {
                    MaxWidth = MaxWidth.ExtraSmall,
                    FullWidth = true,
                    Position = DialogPosition.Center,
                });

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                try
                {
                    var DeleteResult = await Http.DeleteAsync($"api/Permissions/Delete/{Model.Id}");
                    if (DeleteResult.IsSuccessStatusCode)
                    {
                        var JsonResult = await DeleteResult.Content.ReadFromJsonAsync<CallbackResult<Permission>>();
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

        #endregion

    }
}
