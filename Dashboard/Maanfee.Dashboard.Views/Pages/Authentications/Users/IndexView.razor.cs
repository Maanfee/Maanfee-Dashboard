using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Base;
using Maanfee.Dashboard.Views.Base.Services;
using Maanfee.Web.Core;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Pages.Authentications.Users
{
    public partial class IndexView
    {
        private IEnumerable<GetUserViewModel> Data = new List<GetUserViewModel>();
        private MudTable<GetUserViewModel> Table = new();
        private TableStateViewModel<string> TableState = new();
        private string SearchString = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await PermissionService.CheckAuthorizeAsync(PermissionDefaultValue.User.View, PermissionAuthenticationState,
                    AuthorizationService, Navigation);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }
        }

        private async Task<TableData<GetUserViewModel>> ServerData(TableState state)
        {
            try
            {
                state.Page++;

                if (state.PageSize == 0)
                {
                    state.PageSize = 10;
                }

                TableState.state = state;
                TableState.Filter = SearchString;

                var PostResult = await Http.PostAsJsonAsync($"api/Users/PaginationIndex", TableState);
                if (PostResult.IsSuccessStatusCode)
                {
                    var stringcallback = await PostResult.Content.ReadAsStringAsync();
                    var JObjectData = Newtonsoft.Json.Linq.JObject.Parse(stringcallback);

                    var List = JsonConvert.DeserializeObject<List<GetUserViewModel>>(JObjectData["data"]?["list"]?.ToString());
                    int TotalItems = JsonConvert.DeserializeObject<int>(JObjectData["data"]?["totalPages"]?.ToString());

                    Data = List.AsEnumerable().Select((data, index) => new GetUserViewModel
                    {
                        RowNum = ((state.Page - 1) * state.PageSize) + (index + 1),
                        Id = data.Id,
                        Name = data.Name,
                        UserName = data.UserName,
                        RoleName = data.RoleName,
                        PersonalCode = data.PersonalCode,
                        Avatar = data.Avatar,
                        UserDepartmentsPersonalTitle = data.UserDepartmentsPersonalTitle,
                        UserDepartmentsManagementTitle = data.UserDepartmentsManagementTitle,
                    }).ToList();

                    IsTableLoading = false;
                    IsLoaded = true;

                    return new TableData<GetUserViewModel>()
                    {
                        TotalItems = TotalItems,
                        Items = Data
                    };
                }
                else
                {
                    IsLoaded = true;

                    Snackbar.Add(PostResult.Content.ReadAsStringAsync().Result, Severity.Error);
                    IsTableLoading = false;
                    return new TableData<GetUserViewModel>()
                    {
                        Items = Data,
                        TotalItems = 0,
                    };
                }
            }
            catch (Exception ex)
            {
                IsTableLoading = false;
                IsLoaded = true;

                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
                return new TableData<GetUserViewModel>()
                {
                    Items = Data,
                    TotalItems = 0,
                };
            }
        }

        private void OnSearch(string text)
        {
            SearchString = text;
            Table.ReloadServerData();
        }

        private async Task OnRefresh()
        {
            await Table.ReloadServerData();
        }
      
    }
}
