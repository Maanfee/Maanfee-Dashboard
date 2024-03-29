﻿using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Maanfee.Web.Core
{
    public class FileDownloadService
    {
        public FileDownloadService(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Maanfee.Web.Core/JsInterop.js").AsTask());
        }

        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var Module = await moduleTask.Value;
                await Module.DisposeAsync();
            }
        }

        // *******************************************

        public async Task DownloadFileFromStream(string FileName, DotNetStreamReference Stream)
        {
            var Module = await moduleTask.Value;
            await Module.InvokeVoidAsync("DownloadFileFromStream", FileName, Stream);
        }

    }
}
