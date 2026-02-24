namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class CurrentUser
    {
        public bool? IsAuthenticated { get; set; }

        public string? UserName { get; set; }

        public Dictionary<string, string>? Claims { get; set; }

        public string? Id { get; set; }

        public string? Name { get; set; }
    }
}
