using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Domain.Entities.Communications;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Pages.Communications
{
	public partial class Chat
	{
		[Parameter]
		public string IdContact { get; set; }

		private GetUserViewModel ContactUser = new();

		public string CurrentIdUser { get; set; }

		public string CurrentUserName { get; set; }

		public string CurrentName { get; set; }

		private string SearchString { get; set; }

		private List<GetUserViewModel> ChatUsers { get; set; } = new List<GetUserViewModel>();

		#region - Search -

		private async void OnSearchUsers(string text)
		{
			SearchString = text;
			await GetUsersAsync(text);
			StateHasChanged();
		}

		private async void OnSearchChats(string text)
		{
			SearchString = text;
			await GetUsersAsync(text);
			StateHasChanged();
		}

		#endregion

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			await InitSignalRAsync();
			await GetUsersAsync("");

			var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
			CurrentIdUser = user.Claims.Where(a => a.Type == "Id").Select(a => a.Value).FirstOrDefault();
			CurrentUserName = user.Claims.Where(a => a.Type == "UserName").Select(a => a.Value).FirstOrDefault();
			CurrentName = user.Claims.Where(a => a.Type == "Name").Select(a => a.Value).FirstOrDefault();

			if (!string.IsNullOrEmpty(IdContact))
			{
				await GetUserAsync(IdContact);
			}
			//Snackbar.Add($"{CurrentIdUser} : {CurrentUserName}", Severity.Error);
		}

		private async Task GetUserAsync(string IdUser)
		{
			try
			{
				var Callback = await Http.GetFromJsonAsync<CallbackResult<GetUserViewModel>>($"api/Users/GetUserById/{IdUser}");
				if (Callback.Data != null)
				{
					await LoadUserChat(Callback.Data);
				}
				else
				{
					Snackbar.Add(Callback.Error.ToString(), Severity.Error);
				}
			}
			catch (Exception ex)
			{
				Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
			}
		}

		private async Task GetUsersAsync(string SearchString)
		{
			try
			{
				var Callback = await Http.GetFromJsonAsync<CallbackResult<List<GetUserViewModel>>>($"api/Users/GetUsers?Name={SearchString}");
				if (Callback.Data != null)
				{
					ChatUsers = Callback.Data;
				}
				else
				{
					Snackbar.Add(Callback.Error.ToString(), Severity.Error);
				}
			}
			catch (Exception ex)
			{
				Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
			}
		}

		private async Task LoadUserChat(GetUserViewModel User)
		{
			try
			{
				IdContact = User.Id;
				ContactUser = User;
				Navigation.NavigateTo($"chat/{IdContact}");

				var Callback = await Http.GetFromJsonAsync<CallbackResult<List<ChatMessage>>>($"api/Chats/GetUserChats?IdUser={User.Id}");

				if (Callback.Data != null)
				{
					Messages = Callback.Data;

					StateHasChanged();
				}
				else
				{
					Snackbar.Add(Callback.Error.ToString(), Severity.Error);
				}
			}
			catch (Exception ex)
			{
				Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
			}
		}

		#region - SignalR -

		[CascadingParameter]
		public HubConnection HubConnection { get; set; }

		private List<ChatMessage> Messages = new List<ChatMessage>();

		public string CurrentMessage { get; set; }

		private async Task InitSignalRAsync()
		{
			try
			{
				if (HubConnection == null)
				{
					HubConnection = new HubConnectionBuilder().WithUrl(Navigation.ToAbsoluteUri("/signalRHub")).Build();
				}

				if (HubConnection.State == HubConnectionState.Disconnected)
				{
					await HubConnection.StartAsync();
				}

				HubConnection.On<ChatMessage, string>("ReceiveMessage", async (message, userName) =>
				{
					if ((IdContact == message.IdToUser && CurrentIdUser == message.IdFromUser) || (IdContact == message.IdFromUser && CurrentIdUser == message.IdToUser))
					{
						if ((IdContact == message.IdToUser && CurrentIdUser == message.IdFromUser))
						{
							var ChatMessage = new ChatMessage
							{
								Message = message.Message,
								SendDate = message.SendDate,
								IdFromUser = message.IdFromUser,
								IdToUser = message.IdToUser,
								FromUser = new ApplicationUser()
								{
									UserName = CurrentUserName,
									Name = CurrentName,
								},
								ToUser = new ApplicationUser()
								{
									UserName = ContactUser.UserName,
									Name = ContactUser.Name,
								}
							};
							Messages.Add(ChatMessage);

							await HubConnection.SendAsync("ChatNotificationAsync", ChatMessage);
						}
						else if ((IdContact == message.IdFromUser && CurrentIdUser == message.IdToUser))
						{
							Messages.Add(new ChatMessage
							{
								Message = message.Message,
								SendDate = message.SendDate,
								IdFromUser = message.IdFromUser,
								IdToUser = message.IdToUser,
								FromUser = new ApplicationUser()
								{
									UserName = ContactUser.UserName,
									Name = ContactUser.Name,
								},
								ToUser = new ApplicationUser()
								{
									UserName = ContactUser.UserName,
									Name = ContactUser.Name,
								}
							});
						}
						StateHasChanged();
					}
				});
			}
			catch (Exception ex)
			{
				Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
			}
		}

		private async Task SubmitAsync()
		{
			try
			{
				if (!string.IsNullOrEmpty(CurrentMessage) && !string.IsNullOrEmpty(IdContact))
				{
					var ChatHistory = new ChatMessage()
					{
						Message = CurrentMessage,
						IdToUser = IdContact,
						IdFromUser = CurrentIdUser,
						SendDate = DateTime.Now,
					};

					ChatHistory.IdFromUser = CurrentIdUser;
					await HubConnection.SendAsync("SendMessageAsync", ChatHistory, CurrentUserName);

					// ********************************************
					var PostResult = await Http.PostAsJsonAsync($"api/Chats/Create", ChatHistory.TrimString());
					if (PostResult.IsSuccessStatusCode)
					{
						var JsonResult = await PostResult.Content.ReadFromJsonAsync<CallbackResult<ChatMessage>>();
						if (JsonResult.Data != null)
						{
							Snackbar.Add(JsonResult.SuccessMessage ?? DashboardResource.MessageSavedSuccessfully, Severity.Success);
						}
						else
						{
							Snackbar.Add(MessageHandler.ErrorHandler(JsonResult.Error), Severity.Error);
						}
					}
					else
					{
						Snackbar.Add(PostResult.Content.ReadAsStringAsync().Result, Severity.Error);
					}
					// ********************************************

					CurrentMessage = string.Empty;
				}
			}
			catch (Exception ex)
			{
				Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
			}
		}

		private async Task CurrentMessageKeypress(KeyboardEventArgs args)
		{
			if (args.Key == "Enter")
			{
				await SubmitAsync();
			}
		}

		#endregion

	}
}
