using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Maanfee.Web.JSInterop
{
    public class EventHandler
    {
        public EventHandler(IJSInProcessObjectReference eventHandlerObject)
        {
            this.EventHandlerObject = eventHandlerObject;
        }

        public IJSInProcessObjectReference EventHandlerObject;

        public async Task RemoveHandler()
        {
            await EventHandlerObject.InvokeVoidAsync("removeHandler");
        }
    }
}
