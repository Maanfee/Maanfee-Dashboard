using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Core;
using Maanfee.Web.Core;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FilterViewModel = Maanfee.Dashboard.Domain.ViewModels.FilterReleaseViewModel;
using TableViewModel = Maanfee.Dashboard.Domain.ViewModels.GetReleaseViewModel;

namespace Maanfee.Dashboard.Views.Pages.Settings.SysReleases
{
    public partial class SysReleaseMgtView
    {
        private IEnumerable<TableViewModel> Data = new List<TableViewModel>();
        private MudTable<TableViewModel> Table = new();
        private TableStateViewModel<FilterViewModel> TableState = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            try
            {
                await PermissionService.CheckAuthorizeAsync(PermissionDefaultValue.Setting.ReleaseManagementView, AuthenticationState,
                    AuthorizationService, Navigation);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }
        }

        private async Task<TableData<TableViewModel>> ServerData(TableState state, CancellationToken token)
        {
            try
            {
                state.Page++;

                if (state.PageSize == 0)
                {
                    state.PageSize = 10;
                }

                TableState.state = new TableState
                {
                    Page = state.Page,
                    PageSize = state.PageSize,
                    SortDirection = state.SortDirection,
                    SortLabel = state.SortLabel,
                };
                TableState._UserName = AccountStateContainer.UserName;
                TableState._Name = AccountStateContainer.Name;
                if (FilterViewModel != null)
                {
                    TableState.Filter = FilterViewModel;
                }

                var PostResult = await Http.PostAsJsonAsync($"api/SysReleases/PaginationIndex", TableState, token);
                if (PostResult.IsSuccessStatusCode)
                {
                    var stringcallback = await PostResult.Content.ReadAsStringAsync();
                    var JObjectData = Newtonsoft.Json.Linq.JObject.Parse(stringcallback);

                    var List = JsonConvert.DeserializeObject<List<SysRelease>>(JObjectData["data"]?["list"]?.ToString());
                    int TotalItems = JsonConvert.DeserializeObject<int>(JObjectData["data"]?["totalPages"]?.ToString());

                    Data = List.AsEnumerable().Select((data, index) => new TableViewModel
                    {
                        RowNum = ((state.Page - 1) * state.PageSize) + (index + 1),
                        Id = data.Id,
                        Version = data.Version,
                        ReleaseDate = data.ReleaseDate,
                        IsActive = data.IsActive
                    }).ToList();

                    IsTableLoading = false;
                    TableState.Dispose();

                    return new TableData<TableViewModel>()
                    {
                        TotalItems = TotalItems,
                        Items = Data
                    };
                }
                else
                {
                    Snackbar.Add(PostResult.Content.ReadAsStringAsync().Result, Severity.Error);
                    IsTableLoading = false;
                    TableState.Dispose();
                    return new TableData<TableViewModel>()
                    {
                        Items = Data,
                        TotalItems = 0,
                    };
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
                IsTableLoading = false;
                TableState.Dispose();
                return new TableData<TableViewModel>()
                {
                    Items = Data,
                    TotalItems = 0,
                };
            }
        }

        private async Task OnReloadData()
        {
            await Table.ReloadServerData();
        }

        #region - Search -

        private FilterViewModel FilterViewModel = new();

        private async Task OpenSearchDialog()
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add("FilterViewModel", FilterViewModel);

            var dialog = await Dialog.ShowAsync<DialogFilter>(DashboardResource.StringSearch, parameters,
                new DialogOptions()
                {
                    NoHeader = true,
                    MaxWidth = MaxWidth.Small,
                    Position = DialogPosition.Center,
                    FullWidth = true,
                    CloseOnEscapeKey = true,
                });

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                if (result.Data != null)
                {
                    FilterViewModel = (FilterViewModel)result.Data;
                    await Table.ReloadServerData();
                }
            }
        }

        #endregion

        #region - Crudate -

        private async Task OpenCrudateDialog<T>(T Id)
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add("Id", Id);

            var dialog = await Dialog.ShowAsync<DialogCrudate>(string.Empty, parameters,
                new DialogOptions()
                {
                    NoHeader = true,
                    MaxWidth = MaxWidth.ExtraExtraLarge,
                    FullWidth = true,
                    Position = DialogPosition.Center,
                    BackgroundClass = "Dialog-Blur",
                    CloseOnEscapeKey = true,
                });

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                if (result.Data != null)
                {
                    //FilterProfileFileAidTypeYear = (FilterProfileFileAidTypeYear)result.Data;
                    await Table.ReloadServerData();
                }
            }
        }

        #endregion

        #region - Details -

        private async Task OpenDetailsDialog<T>(T Id)
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add("Id", Id);

            var dialog = await Dialog.ShowAsync<DialogDetails>(string.Empty, parameters,
                new DialogOptions()
                {
                    NoHeader = true,
                    MaxWidth = MaxWidth.ExtraLarge,
                    FullWidth = true,
                    Position = DialogPosition.Center,
                    BackgroundClass = "Dialog-Blur",
                    CloseOnEscapeKey = true,
                });
        }

        #endregion

        #region - Delete -

        private async Task OpenDeleteDialog<T>(T Id)
        {
            DialogParameters parameters = new DialogParameters();

            var dialog = await Dialog.ShowAsync<DialogDelete>(DashboardResource.StringAlert, parameters,
                new DialogOptions()
                {
                    MaxWidth = MaxWidth.ExtraSmall,
                    FullWidth = true,
                    Position = DialogPosition.Center,
                    CloseOnEscapeKey = true,
                });

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                try
                {
                    var DeleteResult = await Http.DeleteAsync($"api/SysReleases/Delete/{Id}");
                    if (DeleteResult.IsSuccessStatusCode)
                    {
                        var JsonResult = await DeleteResult.Content.ReadFromJsonAsync<CallbackResult<SysRelease>>();
                        if (JsonResult.Data != null)
                        {
                            Snackbar.Add(JsonResult.SuccessMessage ?? DashboardResource.MessageDeletedSuccessfully, Severity.Success);
                            await Table.ReloadServerData();
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
