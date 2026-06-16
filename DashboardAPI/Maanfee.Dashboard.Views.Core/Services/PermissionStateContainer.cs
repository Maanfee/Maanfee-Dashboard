using Maanfee.Web.Core;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Maanfee.Dashboard.Views.Base
{
    public class PermissionStateContainer
    {
        private List<string> _permissions = new List<string>();
        public event Action? OnChange;

        public List<string> Permissions
        {
            get => _permissions;
            set
            {
                _permissions = value;
                NotifyStateChanged();
            }
        }

        public void HasPermissionToDisplayView(string Permission, NavigationManager Navigation)
        {
            if (!_permissions.Contains(Permission))
            {
                Navigation.NavigateTo("/AccessDeniedView");
            }
        }

        public bool HasPermission(string Permission, bool IsNavigate = false)
        {
            return _permissions.Contains(Permission);
        }

        public async Task LoadPermissionsAsync(HttpClient Http, string IdUser)
        {
            var Callback = await Http.GetFromJsonAsync<CallbackResult<List<string>>>($"api/RoleClaim/GetRoleClaimsByUserId?IdUser={IdUser}");
            Permissions = Callback!.Data!.ToList() ?? new List<string>();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
