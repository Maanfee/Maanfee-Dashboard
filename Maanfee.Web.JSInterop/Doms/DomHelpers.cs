using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maanfee.Web.JSInterop
{
    public static class DomHelpers
    {
		#region - Text -

		public static async Task<Dom> TextAsync(this Task<Dom> dom, string Text) => await (await dom).TextAsync(Text);

        public static async Task<List<string>> TextAsync(this Task<Dom> dom) => await (await dom).TextAsync();

		#endregion

		#region - Css -

		public static async Task<Dom> CssAsync(this Task<Dom> dom, string Property, string Value) => await (await dom).CssAsync(Property, Value);

		public static async Task<List<string>> CssAsync(this Task<Dom> dom, string Property) => await (await dom).CssAsync(Property);

		#endregion

		#region - AddClass -

		public static async Task<Dom> AddClassAsync(this Task<Dom> dom, string Class) => await (await dom).AddClassAsync(Class);

		public static async Task<List<string>> AddClassAsync(this Task<Dom> dom) => await (await dom).AddClassAsync();

		#endregion

		#region - RemoveClass -

		public static async Task<Dom> RemoveClassAsync(this Task<Dom> dom, string Class) => await (await dom).RemoveClassAsync(Class);

		#endregion

		#region - HasClass -

		public static async Task<bool> HasClassAsync(this Task<Dom> dom, string Class) => await (await dom).HasClassAsync(Class);

		#endregion

		public static async Task<EventHandler> AddEventListenerAsync(this Task<Dom> dom, string EventName, Action<object> Handler) => await (await dom).AddEventListenerAsync(EventName, Handler);

        public static async Task<Dom> OnClickAsync(this Task<Dom> dom) => await (await dom).OnClickAsync();

		// *********************** New ***********************

		public static async Task<Dom> ValAsync(this Task<Dom> dom, string text) => await (await dom).ValAsync(text);
		
        public static async Task<List<string>> ValAsync(this Task<Dom> dom) => await (await dom).ValAsync();
	}
}
 