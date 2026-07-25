using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.JsInterop;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Maanfee.Dashboard.Views.Core.Services
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        public JwtAuthenticationStateProvider(HttpClient httpClient, LocalStorage localStorage)
        {
            Http = httpClient;

            LocalStorage = localStorage;
        }

        private readonly HttpClient Http;
        private LocalStorage LocalStorage;
        public event Func<Task>? OnAuthenticationChanged;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await LocalStorage.GetAsync<string>("JwtToken_Service");
            if (!string.IsNullOrEmpty(token) && !IsTokenExpired(token))
            {
                var claims = JwtParser.ParseClaimsFromJwt(token);
                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        private bool IsTokenExpired(string token)
        {
            var claims = JwtParser.ParseClaimsFromJwt(token);
            var expClaim = claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            if (expClaim != null && long.TryParse(expClaim, out long exp))
            {
                var expDate = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
                return expDate < DateTime.UtcNow;
            }
            return true;
        }

        public void NotifyUserAuthentication(string token)
        {
            var claims = JwtParser.ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwtAuthType");
            var authenticatedUser = new ClaimsPrincipal(identity);
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            NotifyAuthenticationStateChanged(authState);
        }

        public async Task<(bool Success, string Message, string Token)> AuthenticationAsync(string Uri, string ServiceName)
        {
            try
            {
                var tokenKey = $"JwtToken_{ServiceName}";
                var existingToken = await LocalStorage.GetAsync<string>(tokenKey);

                if (!string.IsNullOrEmpty(existingToken) && !IsTokenExpired(existingToken))
                {
                    SetAuthorizationHeader(existingToken);
                    NotifyUserAuthentication(existingToken);

                    if (OnAuthenticationChanged != null)
                        await OnAuthenticationChanged.Invoke();

                    return (true, DashboardResource.MessageTokenIsAvailable, existingToken);
                }

                var loginModel = new JwtLoginViewModel
                {
                    UserName = "Maanfee",
                    Password = "Maanfee",
                };

                var response = await Http.PostAsJsonAsync($"{Uri}/{ServiceName}/Accounts/Login", loginModel);

                if (!response.IsSuccessStatusCode)
                {
                    return (false, await response.Content.ReadAsStringAsync(), null)!;
                }

                var result = await response.Content.ReadFromJsonAsync<JwtAuthenticationViewModel>();
                if (result?.IsAuthSuccessful != true || string.IsNullOrEmpty(result.Token))
                {
                    return (false, $"{DashboardResource.MessageUnauthorized} - {result?.ErrorMessage}", null)!;
                }

                await LocalStorage.SetAsync(tokenKey, result.Token);
                SetAuthorizationHeader(result.Token);
                NotifyUserAuthentication(result.Token);

                if (OnAuthenticationChanged != null)
                    await OnAuthenticationChanged.Invoke();

                return (true, DashboardResource.MessageTokenIsAvailable, result.Token);
            }
            catch (Exception ex)
            {
                return (false, $"{DashboardResource.StringError} : {DashboardResource.MessageServiceCommunicationError} - {ex.Message}", null)!;
            }
        }

        private void SetAuthorizationHeader(string token)
        {
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        public async Task LogoutAsync(string serviceName)
        {
            var tokenKey = $"JwtToken_{serviceName}";
            await LocalStorage.RemoveAsync(tokenKey);
            Http.DefaultRequestHeaders.Authorization = null;

            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

    }
}
