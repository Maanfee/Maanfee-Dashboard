namespace Maanfee.Dashboard.Domain.ViewModels
{
	public class SysReleaseFeatureViewModel
	{
		public string Id { get; set; }
		
		public string Comment { get; set; }
	
		public int IdSysReleaseType { get; set; }
		
		public string SysReleaseTypeTitle { get; set; }

        public Nullable<DateTime> FeatureDate { get; set; }
    }
}
