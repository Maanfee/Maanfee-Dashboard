using Maanfee.Dashboard.Core;
using MudBlazor;
using System;
using System.Threading.Tasks;
using Maanfee.Dashboard.Domain.ViewModels;

namespace Maanfee.Dashboard.Views.Pages.Authentications
{
    public partial class Login
    {
        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
        private LoginViewModel LoginViewModelSubmit = new();

        private void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }
      
        private async Task OnSubmit()
        {
            try
            {
                await AuthenticationStateProvider.Login(LoginViewModelSubmit.TrimString());
                Navigation.NavigateTo("");
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }
      
    }
}
