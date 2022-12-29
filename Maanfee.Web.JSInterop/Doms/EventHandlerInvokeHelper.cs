using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
