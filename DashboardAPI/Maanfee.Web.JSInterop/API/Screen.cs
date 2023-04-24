using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Maanfee.Web.JSInterop
{
	public class Screen : ComponentBase, IAsyncDisposable
	{
		#region - JsRuntime -

		public Screen(IJSRuntime JsRuntime)
		{
			moduleTask = new(() => JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Maanfee.Web.JSInterop/Screen.js").AsTask());
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

		public async Task<int> WidthAsync()
		{
			var Module = await moduleTask.Value;
			var data = await Module.InvokeAsync<int>("Width");

			return data;
		}

		public async Task<int> HeightAsync()
		{
			var Module = await moduleTask.Value;
			var data = await Module.InvokeAsync<int>("Height");

			return data;
		}

		public async Task<int> AvailableWidthAsync()
		{
			var Module = await moduleTask.Value;
			var data = await Module.InvokeAsync<int>("AvailableWidth");

			return data;
		}

		public async Task<int> AvailableHeightAsync()
		{
			var Module = await moduleTask.Value;
			var data = await Module.InvokeAsync<int>("AvailableHeight");

			return data;
		}

		public async Task<int> ColorDepthAsync()
		{
			var Module = await moduleTask.Value;
			var data = await Module.InvokeAsync<int>("ColorDepth");

			return data;
		}

		public async Task<int> PixelDepthAsync()
		{
			var Module = await moduleTask.Value;
			var data = await Module.InvokeAsync<int>("PixelDepth");

			return data;
		}

		// *************************************************

		public int Width() => WidthAsync().Result;

		public int Height() => HeightAsync().Result;

		public int AvailableWidth() => AvailableWidthAsync().Result;

		public int AvailableHeight() => AvailableHeightAsync().Result;

		public int ColorDepth() => ColorDepthAsync().Result;

		public int PixelDepth() => PixelDepthAsync().Result;
	}
}
