using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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

			NotifyStateChanged();
		}

		public async Task CloseFullscreenAsync()
		{
			var Module = await moduleTask.Value;
			await Module.InvokeVoidAsync("CloseFullscreen");

			NotifyStateChanged();
		}

		public async Task ToggleFullscreenAsync()
		{
			var Module = await moduleTask.Value;
			await Module.InvokeVoidAsync("ToggleFullscreen");

			NotifyStateChanged();
		}

		private bool _IsFullscreen = false;
		public bool IsFullscreen
		{
			get
			{
				Task.Run(async () => { _IsFullscreen = await IsFullscreenAsync(); });
				return _IsFullscreen;
			}
			set
			{
				_IsFullscreen = value;
				NotifyStateChanged();

				//Task.Run(async () => 
				//            { 
				//_IsFullscreen = IsFullscreenAsync().Result;
				//NotifyStateChanged();
				//});
			}
		}

		public async Task<bool> IsFullscreenAsync()
		{
			var Module = await moduleTask.Value;
			var data = await Module.InvokeAsync<bool>("IsFullscreen");

			return data;
		}

		public event Action OnFullscreenChange;

		private void NotifyStateChanged() => OnFullscreenChange?.Invoke();
	}
}
