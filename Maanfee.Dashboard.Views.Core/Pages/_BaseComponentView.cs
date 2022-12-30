using Maanfee.Dashboard.Views.Core.Extensions;
using Maanfee.Dashboard.Views.Core.Services;
using Maanfee.Web.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Net.Http;

namespace Maanfee.Dashboard.Views.Core
{
    public class _BaseComponentView : ComponentBase, IDisposable
    {
#pragma warning disable CS0108

        [Inject]
        protected HttpClient Http { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        //[Inject]
        //protected CustomStateProvider AuthenticationStateProvider { get; set; }

        //[Inject]
        //protected IJSRuntime JS { get; set; }

        [Inject]
        protected AccountStateContainer AccountStateContainer { get; set; }

        // JWT
        [Inject]
        protected LocalConfiguration LocalConfiguration { get; set; }

        // JWT
        [Inject]
        protected LocalStorage LocalStorage { get; set; }

        // JWT
        [Inject]
        protected JwtAuthenticationStateProvider JwtAuthenticationStateProvider { get; set; }

        [Inject]
        protected IDialogService Dialog { get; set; }

#pragma warning restore CS0108

        // *****************************************

        protected bool IsProcessing = false;

        protected bool IsLoaded = false;

        protected bool IsTableLoading = true;

        // *****************************************

        protected override void OnInitialized()
        {
            AccountStateContainer.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            AccountStateContainer.OnChange -= StateHasChanged;
        }

        // *****************************************

        #region - MudTable Config -

        protected bool _IsTableDense = true;
        protected bool _IsTableFixedHeader = true;
        protected bool _IsTableScroll = true;

        #endregion

        #region - Snackbar Config -

        [Inject]
        protected ISnackbar Snackbar { get; set; }

        #endregion

        #region - Validation Parameters

        protected EditContext EditContext;

        protected ValidationMessageStore ValidationMessageStore;

        protected bool HasValidationError = false;

        #endregion

    }
}
