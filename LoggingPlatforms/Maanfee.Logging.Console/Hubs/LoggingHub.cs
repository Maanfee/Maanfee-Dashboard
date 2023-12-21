using Microsoft.AspNetCore.SignalR;

namespace Maanfee.Logging.Console
{
    public class LoggingHub: Hub
    {
        public async Task SendMessageAsync(LogInfo Message)
        {
            await Clients.All.SendAsync("ReceiveMessage", Message);
        }
    }
}
