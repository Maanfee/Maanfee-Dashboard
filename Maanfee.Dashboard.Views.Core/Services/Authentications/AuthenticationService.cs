using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Views.Core.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public AuthenticationService(HttpClient httpClient,
            JwtAuthenticationStateProvider _JwtAuthenticationStateProvider,
            LocalConfiguration _LocalConfiguration)
        {
            Http = httpClient;

            JwtAuthenticationStateProvider = _JwtAuthenticationStateProvider;
            LocalConfiguration = _LocalConfiguration;
        }

        private readonly HttpClient Http;
        private JwtAuthenticationStateProvider JwtAuthenticationStateProvider;
        private LocalConfiguration LocalConfiguration;

        public async Task<CurrentUser> CurrentUserInfo()
        {
            var result = await Http.GetFromJsonAsync<CurrentUser>("api/Authentications/currentuserinfo");
            return result;
        }

        public async Task Login(LoginViewModel loginRequest)
        {
            var result = await Http.PostAsJsonAsync("api/Authentications/login", loginRequest);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
            result.EnsureSuccessStatusCode();

            #region - Login for messaging -

            //var Model = new JwtLoginViewModel
            //{
            //    UserName = "Maanfee", // loginRequest.UserName,
            //    Password = "Maanfee", // loginRequest.Password,
            //};

            //var PostResult = await Http.PostAsJsonAsync($"{SettingsService.Automation.Url}/accounts/login", Model);
            //if (PostResult.IsSuccessStatusCode)
            //{
            //    var JsonResult = await PostResult.Content.ReadFromJsonAsync<AuthenticationViewModel>();
            //    if (JsonResult != null)
            //    {
            //        await LocalConfiguration.SetAsync(StorageDefaultValue.JwtTokenStorage, JsonResult.Token);
            //        ((JwtAuthenticationStateProvider)JwtAuthenticationStateProvider).NotifyUserAuthentication(loginRequest.UserName);
            //        Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", JsonResult.Token);
            //    }
            //}

            #endregion
        }

        public async Task Logout()
        {
            var result = await Http.PostAsync("api/Authentications/logout", null);
            result.EnsureSuccessStatusCode();
        }

        public async Task Register(RegisterViewModel registerRequest)
        {
            try
            {
                var result = await Http.PostAsJsonAsync("api/Authentications/register", registerRequest);
                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(await result.Content.ReadAsStringAsync());
                }
                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task ResetPassword(ResetPasswordViewModel resetPasswordRequest)
        {
            try
            {
                var result = await Http.PostAsJsonAsync("api/Authentications/resetpassword", resetPasswordRequest);
                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(await result.Content.ReadAsStringAsync());
                }
                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

    }
}
