using MudBlazor;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class TableStateViewModel
    {
        public TableState state { get; set; } = new();

        public string UserName { get; set; }

        public string IdOptional { get; set; }
    }

    public class TableStateViewModel<T> : TableStateViewModel
	{
		public T Filter { get; set; }
    }
}
