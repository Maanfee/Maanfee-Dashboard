using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Core;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Pages.Authentications
{
    public partial class Register
    {
        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

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

        private RegisterViewModel RegisterViewModelSubmit { get; set; } = new RegisterViewModel();

        private async Task OnSubmit()
        {
            if (RegisterViewModelSubmit.IsTermsService)
            {
                try
                {
                    await AuthenticationStateProvider.Register(RegisterViewModelSubmit.TrimString());
                    Navigation.NavigateTo("");
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("is already taken"))
                    {
                        Snackbar.Add($"{RegisterViewModelSubmit.UserName} is already taken", Severity.Error);
                    }
                    else if (ex.Message.Contains("This referral code is not valid."))
                    {
                        Snackbar.Add("This referral code is not valid.", Severity.Error);
                    }
                    else
                    {
                        Snackbar.Add(ex.ToString(), Severity.Error);
                    }
                }
            }
            else
            {
                Snackbar.Add("You must agree Terms of Service.", Severity.Error);
            }
        }

    }
}
