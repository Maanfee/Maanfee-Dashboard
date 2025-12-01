using Maanfee.Dashboard.Core;
using Maanfee.JsInterop;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Maanfee.Dashboard.Views.Core.Services
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        public JwtAuthenticationStateProvider(HttpClient httpClient, LocalStorage localStorage)
        {
            Http = httpClient;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            LocalStorage = localStorage;
        }

        private readonly HttpClient Http;
        private readonly AuthenticationState _anonymous;
        private LocalStorage LocalStorage;

        public string JwtTokenStorage { get; set; }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (string.IsNullOrWhiteSpace(JwtTokenStorage))
            {
                return _anonymous;
            }

            var token = await LocalStorage.GetAsync<string>(JwtTokenStorage);
            if (string.IsNullOrWhiteSpace(token))
                return _anonymous;

            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));
        }

        public void NotifyUserAuthentication(string email)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}