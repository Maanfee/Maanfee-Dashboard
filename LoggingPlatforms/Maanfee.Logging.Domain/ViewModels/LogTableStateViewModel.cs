using MudBlazor;
using System;

namespace Maanfee.Logging.Domain
{
	public class LogTableStateViewModel : IDisposable
    {
        public TableState state { get; set; } = new();

        public string IdOptional { get; set; }

        public virtual void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }

    public class LogTableStateViewModel<T> : LogTableStateViewModel, IDisposable
    {
        public T Filter { get; set; }

        public override void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
