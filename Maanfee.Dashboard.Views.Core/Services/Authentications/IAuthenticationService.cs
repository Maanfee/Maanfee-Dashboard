using Maanfee.Dashboard.Domain.ViewModels;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Core.Services
{
    public interface IAuthenticationService
    {
        Task Login(LoginViewModel loginRequest);
        Task Register(RegisterViewModel registerRequest);
        Task ResetPassword(ResetPasswordViewModel resetPasswordRequest);
        Task Logout();
        Task<CurrentUser> CurrentUserInfo();
    }
}
