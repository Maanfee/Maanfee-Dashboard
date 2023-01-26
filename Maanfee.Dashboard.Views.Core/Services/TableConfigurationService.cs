using Maanfee.Web.JSInterop;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Core.Services
{
	public class TableConfigurationService
	{
		public string SetHeight(bool IsRtl, bool IsFullscreen, bool IsTableScroll)
		{
			//Task.Run(async () => { IsFullscreen = await IsFullscreenMode; });

			if (IsFullscreen && IsTableScroll)
			{
				return "450px";
			}
			if (!IsFullscreen && IsTableScroll)
			{
				return "320px";
			}
			return "100%";
		}

	}
}
