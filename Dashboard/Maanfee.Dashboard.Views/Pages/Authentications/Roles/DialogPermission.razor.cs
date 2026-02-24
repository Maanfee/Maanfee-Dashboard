using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Core;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Pages.Authentications.Roles
{
    public partial class DialogPermission
    {
        [Parameter]
        public string IdRole { get; set; }

        [Parameter]
        public string RoleName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetTitleNamesAsync();

                OnSelectedTab();

                IsLoaded = true;
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }
        }

        private async Task GetTitleNamesAsync()
        {
            await GetRoleClaimViewModelsAsync();

            foreach (var cl in typeof(PermissionDefaultValue).MaanfeeGetMembers())
            {
                string Val = cl.MaanfeeDisplayAttribute().Name.MaanfeeGetResourceValue<DashboardResource>();
                //if(string.IsNullOrEmpty(Val))
                //{
                //    Val = cl.MaanfeeDisplayAttribute().Name.MaanfeeGetResourceValue<AppResource>();
                //}
                TitleNames.Add(cl.Name, Val);
            }
        }

        private void OnSelectedTab(string ClassName = "Settings")
        {
            SubmitRoleClaimViewModels.Clear();

            foreach (var prop in typeof(PermissionDefaultValue).GetNestedTypes()
                          .SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null && propertyValue.ToString().Contains(ClassName))
                {
                    var RoleClaim = new SubmitRoleClaimViewModel();
                    RoleClaim.ClaimType = propertyValue.ToString();
                    var Val = prop.MaanfeeDisplayAttribute().Name.MaanfeeGetResourceValue<DashboardResource>();
                    if (string.IsNullOrEmpty(Val))
                    {
                        //Val = prop.MaanfeeDisplayAttribute().Name.MaanfeeGetResourceValue<AppResource>();
                    }
                    RoleClaim.Action = Val;
                    RoleClaim.ClaimValue = PermissionClaimTypes.Permission;
                    RoleClaim.RoleId = IdRole;

                    var DataFromDb = GetRoleClaimFromDatabase.FirstOrDefault(x => x.ClaimType == propertyValue.ToString());
                    if (DataFromDb != null)
                    {
                        RoleClaim.Id = DataFromDb.Id;
                        //RoleClaim.RoleId = DataFromDb.RoleId;
                        RoleClaim.RoleId = IdRole;
                        RoleClaim.IsSelected = true;
                    }

                    SubmitRoleClaimViewModels.Add(RoleClaim);
                }
            }
        }

        private async Task SaveAsync()
        {
            if (IsProcessing)
                return;
            IsProcessing = true;

            try
            {
                var PostResult = await Http.PostAsJsonAsync("api/RoleClaim/CreateRange", SubmitRoleClaimViewModels);
                if (PostResult.IsSuccessStatusCode)
                {
                    var JsonResult = await PostResult.Content.ReadFromJsonAsync<CallbackResult<IList<SubmitRoleClaimViewModel>>>();
                    if (JsonResult?.Data != null)
                    {
                        await GetRoleClaimViewModelsAsync();
                        // عدم ریست شدن و ماندن سر جای قبلی
                        // *********** Reset ***********
                        //OnSelectedTab();
                        //tabs.ActivatePanel(0);
                        // *****************************
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
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }
            IsProcessing = false;
        }

        private Dictionary<string, string> TitleNames { get; } = new();

        private List<SubmitRoleClaimViewModel> SubmitRoleClaimViewModels = new List<SubmitRoleClaimViewModel>();

        private List<GetRoleClaimViewModel> GetRoleClaimFromDatabase = new();

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

        private MudTabs tabs;
    }
}
