using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Core;
using Maanfee.Web.Core;
using MudBlazor;
using System.Net.Http.Json;

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
			await base.OnInitializedAsync();

			IsLoaded = false;

            try
            {
                await PermissionService.CheckAuthorizeAsync(PermissionDefaultValue.Department.View, AuthenticationState,
                     AuthorizationService, Navigation);

                CanCreate = await PermissionService.IsAuthorizeAsync(PermissionDefaultValue.Department.Create, AuthenticationState,
                    AuthorizationService, Navigation);

                CanDelete = await PermissionService.IsAuthorizeAsync(PermissionDefaultValue.Department.Delete, AuthenticationState,
                      AuthorizationService, Navigation);
                            
                SelectedValue = null;

                await ResetAsync();

            }
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }

            IsLoaded = true;
        }

        #region - Tree Builder -

        //private MudTreeView<Department> TreeView;

        private List<TreeItemData<Department>> TreeItems { get; set; } = new();

        private async Task BuildTreeAsync()
        {
            TreeItems = await BuildTreeItems(null);
        }

        private Task<List<TreeItemData<Department>>> BuildTreeItems(int? parentId)
        {
            var items = new List<TreeItemData<Department>>();

            var departments = Departments.Where(d => d.IdParent == parentId).ToList();

            foreach (var dept in departments)
            {
                var treeItem = new TreeItemData<Department>
                {
                    Value = dept,
                    Text = dept.Title,
                    Expandable = Departments.Any(child => child.IdParent == dept.Id),
                    Icon = Icons.Material.Filled.Groups,
                    Children = new List<TreeItemData<Department>>()
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

        public async Task<IReadOnlyCollection<TreeItemData<Department>>> LoadServerData(Department parentValue)
        {
            try
            {
                if (parentValue == null)
                {
                    // بارگذاری ریشه‌ها
                    var roots = Departments.Where(d => d.IdParent == null).ToList();
                    return roots.Select(d => new TreeItemData<Department>
                    {
                        Value = d,
                        Text = d.Title,
                        Expandable = Departments.Any(child => child.IdParent == d.Id),
                        Children = new List<TreeItemData<Department>>()
                    }).ToList().AsReadOnly();
                }
                else
                {
                    // بارگذاری فرزندان یک والد خاص
                    var children = Departments.Where(d => d.IdParent == parentValue.Id).ToList();
                    return children.Select(d => new TreeItemData<Department>
                    {
                        Value = d,
                        Text = d.Title,
                        Expandable = Departments.Any(child => child.IdParent == d.Id),
                        Children = new List<TreeItemData<Department>>()
                    }).ToList().AsReadOnly();
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{ex.Message}", Severity.Error);
                return new List<TreeItemData<Department>>().AsReadOnly();
            }
        }

        #endregion

        #region - Search -

        //private string SearchPhrase;

        //private async void OnTextChanged(string searchPhrase)
        //{
        //    SearchPhrase = searchPhrase;
        //    Departments = Departments.Where(d => d.Title.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase)).ToList();
        //    await BuildTreeAsync();
        //}

        #endregion

        private async Task OnSubmit()
        {
            if (IsProcessing)
                return;
            IsProcessing = true;

            try
            {
                var PostResult = await Http.PostAsJsonAsync("api/Departments/CreateOrUpdate", SubmitDepartmentViewModel.TrimStringAndCheckPersianSpecialLetter());
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

        private List<DropDownDepartmentViewModel> Parents = new();

        private async Task<IEnumerable<DropDownDepartmentViewModel>> GetParentsAsync(string value, CancellationToken token)
        {
            var Callback = await Http.GetFromJsonAsync<CallbackResult<List<DropDownDepartmentViewModel>>>($"api/Departments/GetDropDownDepartments?value={value}", token);
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

		#endregion

		private List<Department> Departments = new();

		private async Task GetDepartmentsAsync()
		{
			var Callback = await Http.GetFromJsonAsync<CallbackResult<List<Department>>>($"api/Departments/GetDepartments");
			if (Callback.Data != null)
			{
				Departments = Callback.Data;
			}
			else
			{
				Snackbar.Add(Callback.Error.ToString(), Severity.Error);
			}
		}

		private async Task ResetAsync()
        {
            await GetDepartmentsAsync();
            await BuildTreeAsync();
            await GetParentsAsync(string.Empty, new CancellationTokenSource().Token);

            SubmitDepartmentViewModel.Id = null;
            SubmitDepartmentViewModel.Title = string.Empty;
            SubmitDepartmentViewModel.Parent = null;
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

        #endregion

    }
}
