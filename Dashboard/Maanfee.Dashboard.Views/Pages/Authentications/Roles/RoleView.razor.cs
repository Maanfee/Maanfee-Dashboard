using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Dashboard.Views.Base;
using Maanfee.Dashboard.Views.Core.Shared.Dialogs;
using Maanfee.Logging.Console;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FilterViewModel = Maanfee.Dashboard.Domain.ViewModels.FilterRoleViewModel;
using TableViewModel = Maanfee.Dashboard.Domain.ViewModels.GetRoleViewModel;

namespace Maanfee.Dashboard.Views.Pages.Authentications.Roles
{
    public partial class RoleView
	{
		private IEnumerable<TableViewModel> Data = new List<TableViewModel>();
		private MudTable<TableViewModel> Table = new();
		private TableStateViewModel<FilterViewModel> TableState = new();

		private bool _PermissionCreate = false;
		private bool _PermissionEdit = false;
		private bool _PermissionDelete = false;
		private bool _PermissionPermission = false;

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

            if (LoggingHubConnection is not null)
            {
                await LoggingHubConnection.SendAsync("SendMessageAsync", new LogInfo
                {
                    Platform = LoggingPlatformDefaultValue.Client,
                    Message = $"{AccountStateContainer.UserName} ({AccountStateContainer.Name}) is Viewing ({DashboardResource.StringRole})",
                    LogDate = DateTime.Now,
                    Level = LogLevel.Information,
                });
            }

            try
			{
				await PermissionService.CheckAuthorizeAsync(PermissionDefaultValue.Role.View, AuthenticationState,
					AuthorizationService, Navigation);
								
				_PermissionCreate = (await AuthorizationService.AuthorizeAsync(PermissionCurrentUser, PermissionDefaultValue.Role.Create)).Succeeded;
				_PermissionEdit = (await AuthorizationService.AuthorizeAsync(PermissionCurrentUser, PermissionDefaultValue.Role.Edit)).Succeeded;
				_PermissionDelete = (await AuthorizationService.AuthorizeAsync(PermissionCurrentUser, PermissionDefaultValue.Role.Delete)).Succeeded;
				_PermissionPermission = (await AuthorizationService.AuthorizeAsync(PermissionCurrentUser, PermissionDefaultValue.Role.Permission)).Succeeded;
			}
			catch (Exception ex)
			{
				Snackbar.Add($"{DashboardResource.StringError} : " + ex.Message, Severity.Error);
			}
		}

		private async Task<TableData<TableViewModel>> ServerData(TableState state)
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
				TableState.UserName = AccountStateContainer.UserName;
				if (FilterViewModel != null)
				{
					TableState.Filter = FilterViewModel;
				}

				var PostResult = await Http.PostAsJsonAsync($"api/Roles/PaginationIndex", TableState);
				if (PostResult.IsSuccessStatusCode)
				{
					var JsonResult = await PostResult.Content.ReadFromJsonAsync<CallbackResult<PaginatedListViewModel<IdentityRole>>>();

					Data = JsonResult.Data.List.AsEnumerable().Select((data, index) => new TableViewModel
					{
						RowNum = ((state.Page - 1) * state.PageSize) + (index + 1),
						Id = data.Id,
						Name = data.Name,
						NormalizedName = data.NormalizedName,
					}).ToList();

					IsTableLoading = false;
					TableState.Dispose();

					return new TableData<TableViewModel>()
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
					return new TableData<TableViewModel>()
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
				return new TableData<TableViewModel>()
				{
					Items = Data,
					TotalItems = 0,
				};
			}
		}

		private void OnReloadData()
		{
			Table.ReloadServerData();
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
					MaxWidth = MaxWidth.Small,
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
					MaxWidth = MaxWidth.Medium,
					FullWidth = true,
					Position = DialogPosition.Center,
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
					MaxWidth = MaxWidth.Medium,
					FullWidth = true,
					Position = DialogPosition.Center,
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
				});

			var result = await dialog.Result;

			if (!result.Canceled)
			{
				try
				{
					var DeleteResult = await Http.DeleteAsync($"api/Roles/Delete/{Id}");
					if (DeleteResult.IsSuccessStatusCode)
					{
						var JsonResult = await DeleteResult.Content.ReadFromJsonAsync<CallbackResult<IdentityRole>>();
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

        #region - Permission -

        private void OpenPermissionDialog<T>(T Id, string RoleName)
        {
            DialogParameters DialogParameters = new DialogParameters();
            DialogParameters.Add("IdRole", Id);
            DialogParameters.Add("RoleName", RoleName);

            var dialog = Dialog.Show<DialogPermission>(string.Empty, DialogParameters,
                new DialogOptions()
                {
                    NoHeader = true,
                    MaxWidth = MaxWidth.ExtraExtraLarge,
                    FullWidth = true,
                    Position = DialogPosition.Center,
                    ClassBackground = "Dialog-Blur",
                });
        }

        #endregion
    }
}
