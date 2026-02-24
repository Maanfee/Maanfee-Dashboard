using System;

namespace Maanfee.Dashboard.Views.Core.Services
{
	public class UrlStateContainer
	{
		private string prevurl = string.Empty;

		public string PrevUrl
		{
			get
			{
				return prevurl;
			}
			set
			{
				prevurl = value;
				NotifyStateChanged();
			}
		}

		public event Action? OnChange;

		private void NotifyStateChanged()
		{
			this.OnChange?.Invoke();
		}
	}
}
