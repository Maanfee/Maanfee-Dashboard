using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Base;
using Maanfee.Dashboard.Views.Core.Shared.Dialogs;
using Maanfee.Logging.Domain;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FilterViewModel = Maanfee.Dashboard.Domain.ViewModels.FilterUserViewModel;

namespace Maanfee.Dashboard.Views.Pages.Authentications.Users
{
    public partial class UserView
	{
		private IEnumerable<GetUserViewModel> Data = new List<GetUserViewModel>();
		private MudTable<GetUserViewModel> Table = new();
        private TableStateViewModel<FilterViewModel> TableState = new();

        private bool _PermissionCreate = false;
		private bool _PermissionEdit = false;
		private bool _PermissionDelete = false;

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

            if (LoggingHubConnection is not null)
            {
                await LoggingHubConnection.SendAsync("SendMessageAsync", new LogInfo
                {
                    IdLoggingPlatform = LoggingPlatformDefaultValue.Client,
                    Message = $"{AccountStateContainer.UserName} ({AccountStateContainer.Name}) is Viewing ({DashboardResource.StringUser})",
                    LogDate = DateTime.Now,
                    IdLoggingLevel = LoggingLevelDefaultValue.Information,
                });
            }

            try
            {
				await PermissionService.CheckAuthorizeAsync(PermissionDefaultValue.User.View, AuthenticationState,
					AuthorizationService, Navigation);

				_PermissionCreate = (await AuthorizationService.AuthorizeAsync(PermissionCurrentUser, PermissionDefaultValue.User.Create)).Succeeded;
				_PermissionEdit = (await AuthorizationService.AuthorizeAsync(PermissionCurrentUser, PermissionDefaultValue.User.Edit)).Succeeded;
				_PermissionDelete = (await AuthorizationService.AuthorizeAsync(PermissionCurrentUser, PermissionDefaultValue.User.Delete)).Succeeded;
			}
			catch (Exception ex)
			{
				Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);

                if (LoggingHubConnection is not null)
                {
                    await LoggingHubConnection.SendAsync("SendMessageAsync", new LogInfo
                    {
                        IdLoggingPlatform = LoggingPlatformDefaultValue.Client,
                        Message = $"{ex.Message}",
                        LogDate = DateTime.Now,
                        IdLoggingLevel = LoggingLevelDefaultValue.Error,
                    });
                }
            }
		}

        private async Task<TableData<GetUserViewModel>> ServerData(TableState state, CancellationToken token)
        {
			try
			{
				state.Page++;

				if (state.PageSize == 0)
				{
					state.PageSize = 10;
				}

				TableState.state = new TableState
				{
					Page = state.Page,
					PageSize = state.PageSize,
					SortDirection = state.SortDirection,
					SortLabel = state.SortLabel,
				};
                if (FilterViewModel != null)
                {
                    TableState.Filter = FilterViewModel;
                }

                var PostResult = await Http.PostAsJsonAsync($"api/Users/PaginationIndex", TableState, token);
				if (PostResult.IsSuccessStatusCode)
				{
					var JsonResult = await PostResult.Content.ReadFromJsonAsync<CallbackResult<PaginatedListViewModel<GetUserViewModel>>>();

					Data = JsonResult.Data.List.AsEnumerable().Select((data, index) => new GetUserViewModel
					{
						RowNum = ((state.Page - 1) * state.PageSize) + (index + 1),
						Id = data.Id,
						Name = data.Name,
						UserName = data.UserName,
						RoleName = data.RoleName,
						PersonalCode = data.PersonalCode,
						Avatar = data.Avatar,
						UserDepartmentsTitle = data.UserDepartmentsTitle,
					}).ToList();

					IsTableLoading = false;
					TableState.Dispose();

					return new TableData<GetUserViewModel>()
					{
						TotalItems = JsonResult.Data.TotalPages,
						Items = Data
					};
				}
				else
				{
					Snackbar.Add(PostResult.Content.ReadAsStringAsync().Result, Severity.Error);
					IsTableLoading = false;
					TableState.Dispose();
					return new TableData<GetUserViewModel>()
					{
						Items = Data,
						TotalItems = 0,
					};
				}
			}
			catch (Exception ex)
			{
				Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
				IsTableLoading = false;
				TableState.Dispose();
				return new TableData<GetUserViewModel>()
				{
					Items = Data,
					TotalItems = 0,
				};
			}
		}

		private async Task OnReloadData()
		{
			await Table.ReloadServerData();
		}

        #region - Search -

        private FilterViewModel FilterViewModel = new();

        private async Task OpenSearchDialog()
        {
            DialogParameters DialogParameters = new DialogParameters();
            DialogParameters.Add("FilterViewModel", FilterViewModel);

            var dialog = Dialog.Show<DialogFilter>(DashboardResource.StringSearch, DialogParameters,
                new DialogOptions()
                {
                    MaxWidth = MaxWidth.Medium,
                    Position = DialogPosition.Center,
                    FullWidth = true,
                });

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                if (result.Data != null)
                {
                    FilterViewModel = (FilterViewModel)result.Data;
                    await Table.ReloadServerData();
                }
            }
        }

        #endregion

        #region - Crudate -

        private async Task OpenCrudateDialog<T>(T Id)
		{
			DialogParameters DialogParameters = new DialogParameters();
			DialogParameters.Add("Id", Id);

			var dialog = Dialog.Show<DialogCrudate>(string.Empty, DialogParameters,
				new DialogOptions()
				{
					NoHeader = true,
					MaxWidth = MaxWidth.ExtraExtraLarge,
					FullWidth = true,
					Position = DialogPosition.Center,
                    BackgroundClass = "Dialog-Blur",
                });

			var result = await dialog.Result;

			if (!result.Canceled)
			{
				if (result.Data != null)
				{
					//FilterViewModel = (FilterViewModel)result.Data;
					await Table.ReloadServerData();
				}
			}
		}

		#endregion

		#region - Details -

		private void OpenDetailsDialog<T>(T Id)
		{
			DialogParameters DialogParameters = new DialogParameters();
			DialogParameters.Add("Id", Id);

			var dialog = Dialog.Show<DialogDetails>(string.Empty, DialogParameters,
				new DialogOptions()
				{
					NoHeader = true,
					MaxWidth = MaxWidth.ExtraExtraLarge,
					FullWidth = true,
					Position = DialogPosition.Center,
					BackgroundClass = "Dialog-Blur"
				});
		}

		#endregion

		#region - Delete -

		private async Task OpenDeleteDialog<T>(T Id)
		{
			DialogParameters DialogParameters = new DialogParameters();

			var dialog = Dialog.Show<DialogDelete>(DashboardResource.StringAlert, DialogParameters,
				new DialogOptions()
				{
					MaxWidth = MaxWidth.ExtraSmall,
					FullWidth = true,
					Position = DialogPosition.Center,
                    BackgroundClass = "Dialog-Blur",
                });

			var result = await dialog.Result;
			if (!result.Canceled)
			{
				try
				{
					var DeleteResult = await Http.DeleteAsync($"api/Authentications/Delete/{Id}");
					if (DeleteResult.IsSuccessStatusCode)
					{
						var JsonResult = await DeleteResult.Content.ReadFromJsonAsync<CallbackResult<GetUserViewModel>>();
						if (JsonResult.Data != null)
						{
							Snackbar.Add(JsonResult.SuccessMessage ?? DashboardResource.MessageDeletedSuccessfully, Severity.Success);
							await Table.ReloadServerData();
						}
						else
						{
							Snackbar.Add(MessageHandler.ErrorHandler(JsonResult.Error), Severity.Error);
						}
					}
					else
					{
						Snackbar.Add(DeleteResult.Content.ReadAsStringAsync().Result, Severity.Error);
					}
				}
				catch (Exception ex)
				{
					Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
				}
			}
		}

		#endregion

	}
}
