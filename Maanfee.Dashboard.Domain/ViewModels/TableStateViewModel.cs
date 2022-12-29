using MudBlazor;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class TableStateViewModel
    {
        public TableState state { get; set; }

        public string Filter { get; set; }

        public string UserName { get; set; }

        public string IdOptional { get; set; }
    }

    public class TableStateViewModel<T>
    {
        public TableState state { get; set; }

        public T Filter { get; set; }

        public string UserName { get; set; }

        public string IdOptional { get; set; }
    }
}
