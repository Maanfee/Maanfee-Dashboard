using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Booklet
{
    public class Booklet : IAsyncDisposable
    {
        public Booklet(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Maanfee.Dashboard.Views.Booklet/js/JsInterop.js").AsTask());
        }

        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
				//var Module = await moduleTask.Value;
				//await Module.InvokeAsync<Task>("Dispose"); 
				////await Module.DisposeAsync();
				await Task.Delay(100);
			}
        }

        // *******************************************

        public async Task<Booklet> InitAsync()
        {
            var Module = await moduleTask.Value;
            await Module.InvokeAsync<Task>("Init");

            return this;
        }

    }
}
