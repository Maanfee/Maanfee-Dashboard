using System;
using System.Collections.Generic;

namespace Maanfee.Dashboard.Views.Core.Services
{
    public class AccountStateContainer
    {
        private string username = string.Empty;
        public string UserName
        {
            get => username;
            set
            {
                username = value;
                NotifyStateChanged();
            }
        }

        private string name = string.Empty;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyStateChanged();
            }
        }

        private string id = string.Empty;
        public string Id
        { 
            get => id;
            set
            {
                id = value;
                NotifyStateChanged();
            }
        }

        private string personalCode = "Init value";
        public string PersonalCode
        {
            get => personalCode;
            set
            {
                personalCode = value;
                NotifyStateChanged();
            }
        }

        private string avatar = string.Empty;
        public string Avatar
        {
            get => avatar;
            set
            {
                avatar = value;
                NotifyStateChanged();
            }
        }

        private List<int> idUserDepartments = new();
        public List<int> IdUserDepartments
        {
            get => idUserDepartments;
            set
            {
                idUserDepartments = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
