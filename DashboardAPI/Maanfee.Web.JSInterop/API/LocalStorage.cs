using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Maanfee.Web.JSInterop
{
    public partial class LocalStorage : ComponentBase, IAsyncDisposable
    {
        #region - JsRuntime -

        public LocalStorage(IJSRuntime JsRuntime)
        {
            moduleTask = new(() => JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Maanfee.Web.JSInterop/Storage.js").AsTask());
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

        public async Task ClearAsync()
        {
            var Module = await moduleTask.Value;
            await Module.InvokeVoidAsync("Clear");
        }

        public async Task<string> KeyAsync(int Index)
        {
            var Module = await moduleTask.Value;
            return await Module.InvokeAsync<string>("Key", Index);
        }

        public async Task<T> GetAsync<T>(string Key)
        {
            var Module = await moduleTask.Value;
            var data = await Module.InvokeAsync<string>("Get", Key);

            return data == null ? default(T) : JsonSerializer.Deserialize<T>(data);
        }

        public async Task<List<string>> KeysAsync()
        {
            var Module = await moduleTask.Value;
            var data = await Module.InvokeAsync<string>("Keys");

            return data == null ? new List<string>() : JsonSerializer.Deserialize<List<string>>(data);
        }

        public async Task<int?> LengthAsync()
        {
            var Module = await moduleTask.Value;
            var data = await Module.InvokeAsync<int?>("Length");

            return data == null ? default : data;
        }

        public async Task SetAsync<T>(string Key, T value)
        {
            var Module = await moduleTask.Value;
            await Module.InvokeVoidAsync("Set", Key, JsonSerializer.Serialize(value));
        }

        public async Task RemoveAsync(string Key)
        {
            var Module = await moduleTask.Value;
            await Module.InvokeVoidAsync("Remove", Key);
        }
    }
}
