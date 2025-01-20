using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Core;
using Maanfee.Dashboard.Views.Core.DefaultValues;
using Maanfee.Dashboard.Views.Core.Shared.Dialogs;
using Maanfee.Dashboard.Views.Pages.Authentications;
using Maanfee.Dashboard.Views.Pages.Settings;
using Maanfee.Web.Core;
using Maanfee.Web.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Utilities;
using System;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Shared
{
    public partial class AdminLayout : _BaseComponentView, IDisposable
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationState { get; set; }

        public string IsVisible = "d-none";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            try
            {
                var State = (await AuthenticationState).User.Identity;

                if (!State.IsAuthenticated)
                {
                    Navigation.NavigateTo("/login");
                }
                else
                {
                    if (SharedLayoutSettings.IsFullscreenMode)
                    {
                        await Fullscreen.ToggleFullscreenAsync();
                    }

                    var username = State.Name;
                    var Callback = await Http.GetFromJsonAsync<CallbackResult<ApplicationUser>>($"/api/Users/GetUserByUserName/{username}");

                    if (Callback.Data != null)
                    {
                        AccountStateContainer.Id = Callback.Data.Id;
                        AccountStateContainer.UserName = Callback.Data.UserName;
                        AccountStateContainer.Name = Callback.Data.Name;
                        AccountStateContainer.Avatar = "data:image/png;base64," + Convert.ToBase64String(Callback.Data.Avatar);
                        AccountStateContainer.PersonalCode = Callback.Data.PersonalCode;
                        AccountStateContainer.IdUserDepartments = Callback.Data.UserDepartments.Select(x => x.IdDepartment).ToList();

                        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
                        Snackbar.Configuration.PreventDuplicates = true;
                        Snackbar.Add($"{DashboardResource.StringWelcome}", Severity.Success);

                        // ********************************************
                        if (!AccountStateContainer.IdUserDepartments.Any())
                        {
                            await Task.Delay(1000);
                            await Dialog.ShowAsync<DialogDepartmentNotFound>(string.Empty,
                                    new DialogOptions
                                    {
                                        BackdropClick = false,
                                        MaxWidth = MaxWidth.Small,
                                        FullWidth = true
                                    });
                        }
                        // ********************************************

                        IsVisible = " ";
                    }
                    else
                    {
                        Snackbar.Add(Callback.Error.ToString(), Severity.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
            }

            Fullscreen.OnFullscreenChange += FullscreenChanged;
        }

        public void Dispose()
        {
            Fullscreen.OnFullscreenChange -= FullscreenChanged;
        }

        // ******************************************************

        private async Task LogoutClick()
        {
            var DialogParameters = new DialogParameters
            {
                {nameof(DialogLogout.ContentText), DashboardResource.StringReallyLogout},
                {nameof(DialogLogout.SubmitButtonColor), Color.Error},
            };

            var dialog = await Dialog.ShowAsync<DialogLogout>(DashboardResource.StringLogout, DialogParameters,
               new DialogOptions()
               {
                   CloseButton = false,
                   MaxWidth = MaxWidth.Small,
                   FullWidth = true,
                   Position = DialogPosition.Center,
                   BackgroundClass = "Dialog-Blur",
                   CloseOnEscapeKey = true,
               });
        }

        private async Task OpenConfigurationDialog()
        {
            DialogParameters DialogParameters = new DialogParameters();

            var dialog = await Dialog.ShowAsync<DialogConfiguration>(string.Empty, DialogParameters,
                new DialogOptions()
                {
                    MaxWidth = MaxWidth.Medium,
                    Position = DialogPosition.Center,
                    FullWidth = true,
                });
        }

        // ******************************************************

        private bool DrawerOpen = true;

        // ******************************************************

        private async Task ToggleDarkMode()
        {
            if (SharedLayoutSettings.IsDarkMode)
            {
                SharedLayoutSettings.IsDarkMode = false;
            }
            else
            {
                SharedLayoutSettings.IsDarkMode = true;
            }

            await LocalConfiguration.SetConfigurationAsync();
        }

        // ******************************************************

        private async Task ToggleDirection()
        {
            if (LanguageModel.IsRTL)
            {
                LanguageModel.IsRTL = false;
            }
            else
            {
                LanguageModel.IsRTL = true;
            }

            SharedLayoutSettings.IsRTL = LanguageModel.IsRTL;

            await LocalStorage.SetAsync<LanguageModel>(StorageDefaultValue.CultureStorage, LanguageModel);
        }

        private async Task ToggleFullscrren(bool Toggled)
        {
            if (Toggled)
            {
                await Fullscreen.ToggleFullscreenAsync();
            }
            else
            {
                await Fullscreen.CloseFullscreenAsync();
            }
        }

        private async void FullscreenChanged()
        {
            TableHeight = TableConfiguration.SetHeight(SharedLayoutSettings.IsRTL, await Fullscreen.IsFullscreenAsync(), _IsTableScroll);

            await InvokeAsync(StateHasChanged);
        }

        // ******************************************************

        private bool IshemingDrawerOpen;

        protected async void ThemingDrawerOpenChangedHandler(bool state)
        {
            IshemingDrawerOpen = state;
            // Save for fullscreen mode
            if (!state)
            {
                await LocalConfiguration.SetConfigurationAsync();
            }
        }

        private void UpdateUserPreferences(MudColor Color)
        {
            CurrentTheme = MaanfeeTheme.ThemeBuilder(SharedLayoutSettings.IsRTL, SharedLayoutSettings.IsDarkMode, Color);
        }

        // ******************************************************

        #region - User Account -

        private async Task OpenUserAccountDialog()
        {
            DialogParameters DialogParameters = new DialogParameters();

            var dialog = await Dialog.ShowAsync<DialogUserAccount>(string.Empty, DialogParameters,
                new DialogOptions()
                {
                    NoHeader = true,
                    MaxWidth = MaxWidth.ExtraLarge,
                    FullWidth = true,
                    Position = DialogPosition.Center,
                    BackgroundClass = "Dialog-Blur"
                });
        }

        #endregion

    }
}
