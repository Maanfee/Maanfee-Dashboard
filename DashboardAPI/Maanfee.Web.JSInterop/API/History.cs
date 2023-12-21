using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Maanfee.Web.JSInterop
{
    public class History : ComponentBase, IAsyncDisposable
    {
        #region - JsRuntime -

        public History(IJSRuntime JsRuntime)
        {
            moduleTask = new(() => JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Maanfee.Web.JSInterop/History.js").AsTask());
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

        public async Task<int> LengthAsync()
        {
            var Module = await moduleTask.Value;
            var data = await Module.InvokeAsync<int>("Length");

            return data;
        }

        public async Task BackAsync()
        {
            var Module = await moduleTask.Value;
            await Module.InvokeVoidAsync("Back");
        }

        public async Task ForwardAsync()
        {
            var Module = await moduleTask.Value;
            await Module.InvokeVoidAsync("Forward");
        }

        public async Task GoAsync(int Index)
        {
            var Module = await moduleTask.Value;
            await Module.InvokeVoidAsync("Go", Index);
        }

        // *************************************************
    }
}
