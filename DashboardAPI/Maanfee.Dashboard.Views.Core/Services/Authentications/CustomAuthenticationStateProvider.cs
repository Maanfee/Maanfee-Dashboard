using Maanfee.Dashboard.Domain.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Maanfee.Dashboard.Views.Core.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public CustomAuthenticationStateProvider(HttpClient httpClient)
        {
            Http = httpClient;
        }

        private CurrentUser _currentUser;
        private readonly HttpClient Http;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await GetCurrentUser();
                if (userInfo.IsAuthenticated)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, _currentUser.UserName),
                    }
                    .Concat(_currentUser.Claims.Select(c => new Claim(c.Key, c.Value)));

                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private async Task<CurrentUser> GetCurrentUser()
        {
            if (_currentUser != null && _currentUser.IsAuthenticated)
            {
                return _currentUser;
            }
            _currentUser = await Http.GetFromJsonAsync<CurrentUser>("api/Authentications/currentuserinfo");
            return _currentUser;
        }

        public async Task Logout()
        {
            var result = await Http.PostAsync("api/Authentications/logout", null);
            result.EnsureSuccessStatusCode();
            _currentUser = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Login(LoginViewModel loginParameters)
        {
            var result = await Http.PostAsJsonAsync("api/Authentications/login", loginParameters);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
            result.EnsureSuccessStatusCode();

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Register(RegisterViewModel registerParameters)
        {
            try
            {
                var result = await Http.PostAsJsonAsync("api/Authentications/register", registerParameters);
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
            finally
            {
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
        }

        public async Task ResetPassword(ResetPasswordViewModel resetParameters)
        {
            try
            {
                var result = await Http.PostAsJsonAsync("api/Authentications/resetpassword", resetParameters);
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
            finally
            {
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
        }
    }
}
