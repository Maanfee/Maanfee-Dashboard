namespace Maanfee.Dashboard.Views.Core.Services
{
	public class TableConfigurationService
	{ 
		public static string InitTableHeight { get; } = "400px";

        public static string InitTableHeightLarge { get; } = "450px";

        public string SetHeight(bool IsRtl, bool IsFullscreen, bool IsTableScroll)
		{
			//Task.Run(async () => { IsFullscreen = await IsFullscreenMode; });

			if (IsFullscreen && IsTableScroll)
			{
				return "520px";
			}
			if (!IsFullscreen && IsTableScroll)
			{
				return InitTableHeight;
			}
			return "100%";
		}


    }
}
