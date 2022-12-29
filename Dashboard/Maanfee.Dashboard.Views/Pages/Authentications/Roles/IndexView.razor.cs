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

namespace Maanfee.Dashboard.Views.Pages.Authentications.Roles
{
    public partial class IndexView
    {
        private IEnumerable<GetRoleViewModel> Data = new List<GetRoleViewModel>();

        private IEnumerable<GetRoleViewModel> PagedData;
        private MudTable<GetRoleViewModel> Table = new();

        private int TotalItems = 0;
        private string SearchString = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            try
            {
                await PermissionService.CheckAuthorizeAsync(PermissionDefaultValue.Role.View, PermissionAuthenticationState,
                    AuthorizationService, Navigation);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }
        }

        private async Task<TableData<GetRoleViewModel>> ServerData(TableState state)
        {
            try
            {
                var Callback = await Http.GetFromJsonAsync<CallbackResult<List<GetRoleViewModel>>>($"api/Roles/Index");
                if (Callback.Data != null)
                {
                    Data = Callback.Data;

                    Data = Data.Where(element =>
                    {
                        if (string.IsNullOrWhiteSpace(SearchString))
                            return true;
                        if (element.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                            return true;
                        return false;
                    }).ToArray();
                    TotalItems = Data.Count();

                    switch (state.SortLabel)
                    {
                        case "Name":
                            Data = Data.OrderByDirection(state.SortDirection, o => o.Name);
                            break;
                        case "NormalizedName":
                            Data = Data.OrderByDirection(state.SortDirection, o => o.NormalizedName);
                            break;
                    }

                    Data = Data.AsEnumerable().Select((data, index) => new GetRoleViewModel
                    {
                        RowNum = index + 1,
                        Id = data.Id,
                        Name = data.Name,
                        NormalizedName = data.NormalizedName,
                    });

                    PagedData = Data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();

                    IsTableLoading = false;

                    return new TableData<GetRoleViewModel>() { TotalItems = TotalItems, Items = PagedData };
                }
                else
                {
                    IsTableLoading = false;

                    Snackbar.Add(Callback.Error.ToString(), Severity.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                IsTableLoading = false;

                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
                return null;
            }
        }

        private void OnSearch(string text)
        {
            SearchString = text;
            Table.ReloadServerData();
        }

        private void OnRefresh()
        {
            Table.ReloadServerData();
        }

    }
}
