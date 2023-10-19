using Maanfee.Dashboard.Domain.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Services
{
    public class LoggingHub: Hub
    {
        public async Task SendMessageAsync(LogInfo Message, string userName)
        {
            await Clients.All.SendAsync("ReceiveMessage", Message, userName);
        }
    }
}