using Microsoft.JSInterop;

namespace Maanfee.Web.Printing
{
    public partial class PrintingService : IPrintingService, IAsyncDisposable
    {
        public PrintingService(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Maanfee.Web.Printing/JsInterop.js").AsTask());
        }

        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        //private IJSObjectReference Module;
        //private readonly IJSRuntime JsRuntime;
 
        //public PrintingService(IJSRuntime jsRuntime)
        //{
        //    this.JsRuntime = jsRuntime;

        //    if (Module is null)
        //    {
        //        Task.Run(async () => await ImportModule());
        //    }
        //}

        //internal async ValueTask ImportModule()
        //{
        //    Module = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Maanfee.Web.Printing/JsInterop.js");
        //}

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var Module = await moduleTask.Value;
                await Module.DisposeAsync();
            }
        }

        // *******************************************

        public async Task PrintAsync(bool IsBackward = true)
        {
            if (PrintSetting.AutoPrint == "Yes")
            {
                var Module = await moduleTask.Value;
                await Module.InvokeAsync<bool>("printwindow", IsBackward);
            }
        }

        public async Task AddClassAsync()
        {
            var Module = await moduleTask.Value;
            await Module.InvokeAsync<bool>("addClass", new[] { PrintSetting.PageSize, PrintSetting.IsLandscape });
        }

        public async Task RemoveClassAsync()
        {
            var Module = await moduleTask.Value;
            await Module.InvokeAsync<bool>("removeClass", new[] { PrintSetting.PageSize, PrintSetting.IsLandscape });
        }             

    }
}
