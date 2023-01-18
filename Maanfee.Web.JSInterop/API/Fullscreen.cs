using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Maanfee.Web.JSInterop
{
    public partial class Fullscreen : ComponentBase, IAsyncDisposable
    {
        #region - JsRuntime -

        public Fullscreen(IJSRuntime JsRuntime)
        {
            moduleTask = new(() => JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Maanfee.Web.JSInterop/Fullscreen.js").AsTask());
        }

        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                //    var Module = await moduleTask.Value;
                //        await Module.DisposeAsync();
                await Task.Delay(100);
            }
        }

        #endregion

        public async Task OpenFullscreenAsync(string Id)
        {
            var Module = await moduleTask.Value;
            await Module.InvokeVoidAsync("OpenFullscreen", Id);
        }

        public async Task CloseFullscreenAsync()
        {
            var Module = await moduleTask.Value;
            await Module.InvokeVoidAsync("CloseFullscreen");
        }

        public async Task ToggleFullscreenAsync()
        {
            var Module = await moduleTask.Value;
            await Module.InvokeVoidAsync("ToggleFullscreen");
        }

		public async Task<bool> IsFullscreenAsync()
		{
			var Module = await moduleTask.Value;
			var data = await Module.InvokeAsync<bool>("IsFullscreen");

			return data;
		}
	}
}
