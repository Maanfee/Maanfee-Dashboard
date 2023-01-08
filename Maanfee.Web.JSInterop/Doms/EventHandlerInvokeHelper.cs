using Microsoft.JSInterop;
using System;

namespace Maanfee.Web.JSInterop
{
    public class EventHandlerInvokeHelper
    {
        private Action<object> action;

        public EventHandlerInvokeHelper(Action<object> action)
        {
            this.action = action;
        }

        [JSInvokable("InvokeHandler")]
        public void InvokeHandler(object value)
        {
            action.Invoke(value);
        }
    }
}
