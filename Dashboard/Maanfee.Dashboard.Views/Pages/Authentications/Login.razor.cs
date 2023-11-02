using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Logging.Console;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Pages.Authentications
{
    public partial class Login
    {
        private bool PasswordVisibility;
        private InputType PasswordInput = InputType.Password;
        private string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        private LoginViewModel LoginViewModelSubmit = new();
        private bool IsProcessing = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private void TogglePasswordVisibility()
        {
            if (PasswordVisibility)
            {
                PasswordVisibility = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                PasswordVisibility = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }

        private async Task OnSubmit()
        {
            if (IsProcessing)
                return;
            IsProcessing = true;

            try
            {
                await AuthenticationStateProvider.Login(LoginViewModelSubmit.TrimStringAndCheckPersianSpecialLetter());
                Navigation.NavigateTo("");
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);

                if (LoggingHubConnection is not null)
                {
                    await LoggingHubConnection.SendAsync("SendMessageAsync", new LogInfo
                    {
                        Platform = LoggingPlatformDefaultValue.Client,
                        Message = $"{ex.Message}",
                        LogDate = DateTime.Now,
                        Level = LogLevel.Error,
                    });
                }
            }

            IsProcessing = false;
        }

    }
}
