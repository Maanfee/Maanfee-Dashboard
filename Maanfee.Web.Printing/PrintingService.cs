using Microsoft.JSInterop;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Maanfee.Web.Printing
{
    public class PrintingService : IPrintingService, IAsyncDisposable
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

        public async Task Print()
        {
            if (PrintSetting.AutoPrint == "Yes")
            {
                var Module = await moduleTask.Value;
                await Module.InvokeAsync<bool>("printwindow", null);
            }
        }

        public async Task AddClass()
        {
            var Module = await moduleTask.Value;
            await Module.InvokeAsync<bool>("addClass", new[] { PrintSetting.PageSize, PrintSetting.IsLandscape });
        }

        public async Task RemoveClass()
        {
            var Module = await moduleTask.Value;
            await Module.InvokeAsync<bool>("removeClass", new[] { PrintSetting.PageSize, PrintSetting.IsLandscape });
        }             

    }
}
