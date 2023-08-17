using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Maanfee.Web.JSInterop
{
    public partial class Dom : ComponentBase, IAsyncDisposable
	{
		#region - JsRuntime -

		public Dom(IJSRuntime JsRuntime)
		{
			//this.Element = new DomElement();
			moduleTask = new(() => JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Maanfee.Web.JSInterop/JsInterop.js").AsTask());
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

		// /_content/Maanfee.Web.JSInterop/JsInterop.js

		#region - Selectors -

		//public DomElement Element { get; private set; }

		//public List<DomElement> Elements { get; private set; }

		private string CurrentSelector { get; set; }

		//https://usefulangle.com/post/10/jquery-to-pure-vanilla-javascript-part-1-selectors
		//https://dev.to/rfornal/-replacing-jquery-with-vanilla-javascript-1k2g


		//    //    //throw new Exception("Logfile cannot be read-only");

		//    //    //return new BlazorQueryDOM(JsRuntime)
		//    //    //{
		//    //    //    CurrentSelector = selector,
		//    //    //    //Elements = JsonSerializer.Deserialize<List<BlazorQueryDOMElement>>(data)
		//    //    //};

		public async Task<Dom> QuerySelector(string Selector)
		{
			var Module = await moduleTask.Value;

			//if (!string.IsNullOrEmpty(Selector))
			//{
			//	var data = await Module.InvokeAsync<string>("QuerySelector", Selector);
			//}

			this.CurrentSelector = Selector;
			//this.Element = JsonSerializer.Deserialize<DomElement>(data);
			return this;
		}

		public async Task<Dom> QuerySelectorAll(string Selector)
		{
			var Module = await moduleTask.Value;

			//if (!string.IsNullOrEmpty(Selector))
			//{
			//	var data = await Module.InvokeAsync<string>("QuerySelectorAll", Selector);
			//}

			this.CurrentSelector = Selector;
			//this.Elements = JsonSerializer.Deserialize<List<DomElement>>(data);

			return this;
		}

		public async Task<Dom> Select(string selector)
		{
			var Module = await moduleTask.Value;
			var data = await Module.InvokeAsync<string>("QuerySelectorAll", selector);

			this.CurrentSelector = selector;
			//this.Elements = JsonSerializer.Deserialize<List<DomElement>>(data);

			return this;
		}

		#endregion

		#region - Text -
	
		public async Task<Dom> TextAsync(string Text)
		{
			var Module = await moduleTask.Value;

			if(!string.IsNullOrEmpty(Text))
			{
				await Module.InvokeAsync<Task>("Text", CurrentSelector, Text);
			}

			return this;
		}

		public async Task<List<string>> TextAsync()
		{
			var Module = await moduleTask.Value;

			var data = await Module.InvokeAsync<string>("Text", CurrentSelector, string.Empty);

			if (!string.IsNullOrEmpty(data))
			{
				return JsonSerializer.Deserialize<List<string>>(data);
			}
			return new List<string>();
		}

		#endregion

		#region - Css -

		public async Task<Dom> CssAsync(string Property, string Value)
		{
			var Module = await moduleTask.Value;

			if (!string.IsNullOrEmpty(Property) || !string.IsNullOrEmpty(Value))
			{
				await Module.InvokeAsync<Task>("Css", CurrentSelector, Property, Value);
			}

			return this;
		}

		public async Task<List<string>> CssAsync(string Property)
		{
			var Module = await moduleTask.Value;
			var data = await Module.InvokeAsync<string>("Css", CurrentSelector, Property, string.Empty);

			if (!string.IsNullOrEmpty(data))
			{
				return JsonSerializer.Deserialize<List<string>>(data);
			}
			return new List<string>();
		}

		#endregion

		#region - AddClass -

		public async Task<Dom> AddClassAsync(string Class)
		{
			var Module = await moduleTask.Value;

			if (!string.IsNullOrEmpty(Class))
			{
				await Module.InvokeAsync<Task>("AddClass", CurrentSelector, Class.Trim());
			}

			return this;
		}

		public async Task<List<string>> AddClassAsync()
		{
			var Module = await moduleTask.Value;
			var data = await Module.InvokeAsync<string>("AddClass", CurrentSelector, string.Empty);

			if (!string.IsNullOrEmpty(data))
			{
				return JsonSerializer.Deserialize<List<string>>(data);
			}
			return new List<string>();
		}

		#endregion

		#region - RemoveClass -

		public async Task<Dom> RemoveClassAsync(string Class)
		{
			var Module = await moduleTask.Value;

			if (!string.IsNullOrEmpty(Class))
			{
				await Module.InvokeAsync<Task>("RemoveClass", CurrentSelector, Class.Trim());
			}

			return this;
		}

		#endregion

		#region - HasClass -

		public async Task<bool> HasClassAsync(string Class)
		{
			var Module = await moduleTask.Value;

			var data = await Module.InvokeAsync<bool>("HasClass", CurrentSelector, Class.Trim());

			return data;
		}

		#endregion

		public async Task<EventHandler> AddEventListenerAsync(string EventName, Action<object> Handler)
		{
			var options = new
			{
				EventName,
				CurrentSelector,
			};

			EventHandlerInvokeHelper eventHandlerInvokeHelper = new EventHandlerInvokeHelper(Handler);

			var Module = await moduleTask.Value;
			var eventHandleObject = await Module.InvokeAsync<IJSInProcessObjectReference>("AddEventListener", DotNetObjectReference.Create(eventHandlerInvokeHelper), options);

			return new EventHandler(eventHandleObject);
		}

		public async Task<Dom> OnClickAsync()
		{
			var Module = await moduleTask.Value;
			await Module.InvokeAsync<Task>("OnClick", CurrentSelector);
			return this;
		}

		// *********************** New ***********************

		public async Task<Dom> ValAsync(string Val)
		{
			var Module = await moduleTask.Value;

			await Module.InvokeAsync<Task>("Val", CurrentSelector, Val);
			return this;
		}

		public async Task<List<string>> ValAsync()
		{
			var Module = await moduleTask.Value;
			var data = await Module.InvokeAsync<string>("Val", CurrentSelector, string.Empty);

			return JsonSerializer.Deserialize<List<string>>(data);
		}

	}
}
