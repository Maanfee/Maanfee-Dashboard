﻿using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Core;
using Maanfee.Dashboard.Views.Core.DefaultValues;
using Maanfee.Dashboard.Views.Core.Shared;
using Maanfee.Dashboard.Views.Core.Shared.Dialogs;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using MudBlazor.Utilities;
using System;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Shared
{
    public partial class AdminLayout : SharedLayout
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationState { get; set; }

        public string IsVisible = "d-none";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await InitSignalRAsync();

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
                            Dialog.Show<DialogDepartmentNotFound>(string.Empty,
                                new DialogOptions
                                {
                                    DisableBackdropClick = true,
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
        }

        // ******************************************************

        private async Task LogoutClick()
        {
            var parameters = new DialogParameters
            {
                {nameof(DialogLogout.ContentText), DashboardResource.StringReallyLogout},
                {nameof(DialogLogout.SubmitButtonColor), Color.Error},
            };

            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true };

            Dialog.Show<DialogLogout>(DashboardResource.StringLogout, parameters, options);

            await Task.Delay(10);
        }

        // ******************************************************

        private bool DrawerOpen = true;

        private void DrawerToggle()
        {
            DrawerOpen = !DrawerOpen;
        }

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

        #region - SignalR Connection Status -

        [CascadingParameter]
        public HubConnection HubConnection { get; set; }

        private async Task InitSignalRAsync()
        {
            if (HubConnection == null)
            {
                HubConnection = new HubConnectionBuilder()
                    .WithUrl(Navigation.ToAbsoluteUri("/signalRHub"))
                    .WithAutomaticReconnect()
                    .Build();
            }

            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

            IDialogReference dialog = null;

            HubConnection.Closed += async (Error) =>
            {
                await Task.Run(() =>
                {
                    DialogParameters DialogParameters = new DialogParameters();

                    dialog = Dialog.Show<DialogServerConnection>(string.Empty, DialogParameters,
                        new DialogOptions()
                        {
                            NoHeader = true,
                            MaxWidth = MaxWidth.Small,
                            FullWidth = true,
                            //FullScreen = true,
                            Position = DialogPosition.Center,
                            CloseOnEscapeKey = true,
                            ClassBackground = "Dialog-Blur",
                            DisableBackdropClick = true,
                        });
                });
            };

            HubConnection.Reconnected += async (Message) =>
            {
                await Task.Run(() =>
                            {
                                if (dialog != null)
                                {
                                    dialog.Close();
                                }
                            });
            };


        }

        #endregion
    }
}
