using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Core.Services
{
    public class PermissionService
    {
        public async Task CheckAuthorizeAsync(string Permission, Task<AuthenticationState> PermissionAuthenticationState,
            IAuthorizationService AuthorizationService, NavigationManager Navigation)
        {
            var PermissionCurrentUser = (await PermissionAuthenticationState).User;
            var CanAccess = (await AuthorizationService.AuthorizeAsync(PermissionCurrentUser, Permission)).Succeeded;

            if (CanAccess == false)
            {
                Navigation.NavigateTo("/AccessDeniedView");
            }
        }

        public async Task<bool> IsAuthorizeAsync(string Permission, Task<AuthenticationState> PermissionAuthenticationState,
           IAuthorizationService AuthorizationService, NavigationManager Navigation)
        {
            var PermissionCurrentUser = (await PermissionAuthenticationState).User;
            return (await AuthorizationService.AuthorizeAsync(PermissionCurrentUser, Permission)).Succeeded;
        }
    }

}
