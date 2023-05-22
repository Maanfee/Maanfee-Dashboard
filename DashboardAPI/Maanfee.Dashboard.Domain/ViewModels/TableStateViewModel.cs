using MudBlazor;
using System;

namespace Maanfee.Dashboard.Domain.ViewModels
{
	public class TableStateViewModel : IDisposable
	{
		public TableState state { get; set; } = new();

		public string UserName { get; set; }

		public string IdOptional { get; set; }

		public virtual void Dispose()
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}
	}

	public class TableStateViewModel<T> : TableStateViewModel, IDisposable
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
