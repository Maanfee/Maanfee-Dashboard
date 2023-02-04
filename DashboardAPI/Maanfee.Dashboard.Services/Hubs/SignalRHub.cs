using Maanfee.Dashboard.Domain.Entities.Communications;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Services
{
	public class SignalRHub : Hub
	{
		public async Task SendMessageAsync(ChatMessage Message, string userName)
		{
			await Clients.All.SendAsync("ReceiveMessage", Message, userName);
		}

		public async Task ChatNotificationAsync(ChatMessage ChatMessage)
		{
			await Clients.All.SendAsync("ReceiveChatNotification", ChatMessage);
		}
	}
}
