using Maanfee.Web.Core;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Maanfee.Dashboard.Views.Base
{
    public class PermissionStateContainer
    {
        private List<string> _permissions = new List<string>();
        private TaskCompletionSource<bool> _permissionsLoaded = new TaskCompletionSource<bool>();
        public event Action? OnChange;

        public List<string> Permissions
        {
            get => _permissions;
            set
            {
                _permissions = value;
                _permissionsLoaded.TrySetResult(true);
                NotifyStateChanged();
            }
        }

        public async Task WaitForPermissionsAsync()
        {
            await _permissionsLoaded.Task;
        }

        public void HasPermissionToDisplayView(string Permission, NavigationManager Navigation)
        {
            if (!_permissionsLoaded.Task.IsCompleted)
                return;

            if (!_permissions.Contains(Permission))
            {
                Navigation.NavigateTo("/AccessDeniedView");
            }
        }

        public bool HasPermission(string Permission, bool IsNavigate = false)
        {
            if (!_permissionsLoaded.Task.IsCompleted)
                return false;

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
