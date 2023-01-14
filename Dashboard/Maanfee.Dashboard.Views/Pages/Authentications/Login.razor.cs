using Maanfee.Dashboard.Core;
using MudBlazor;
using System;
using System.Threading.Tasks;
using Maanfee.Dashboard.Domain.ViewModels;

namespace Maanfee.Dashboard.Views.Pages.Authentications
{
    public partial class Login
    {
        private bool PasswordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
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
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                PasswordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }
      
        private async Task OnSubmit()
        {
            if (IsProcessing)
                return;
            IsProcessing = true;

            try
            {
                await AuthenticationStateProvider.Login(LoginViewModelSubmit.TrimString());
                Navigation.NavigateTo("");
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }

            IsProcessing = false;
        }

    }
}
