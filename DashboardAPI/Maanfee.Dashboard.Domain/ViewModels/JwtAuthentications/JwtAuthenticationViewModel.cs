namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class JwtAuthenticationViewModel
    {
        public bool IsAuthSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
    }
}
