using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using MudBlazor;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Pages.Authentications
{
    public partial class AccountView
    {
        public SubmitChangePasswordViewModel SubmitChangePasswordViewModel = new();

		protected override Task OnInitializedAsync()
		{
			return base.OnInitializedAsync();
		}

		private async Task OnSubmit()
        {
            if (IsProcessing)
                return;
            IsProcessing = true;

            try
            {
                var PostResult = await Http.PostAsJsonAsync("api/Authentications/ChangePassword", SubmitChangePasswordViewModel.TrimString());
                if (PostResult.IsSuccessStatusCode)
                {
                    Snackbar.Add(DashboardResource.MessageSavedSuccessfully, Severity.Success);

                    await Task.Delay(1000);

                    await AuthenticationStateProvider.Logout();
                    Navigation.NavigateTo("/login");
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

    }
}
