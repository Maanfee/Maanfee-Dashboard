using MudBlazor;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class TableStateViewModel : _BaseViewModel
    {
        public TableState state { get; set; } = new();

        public string? IdOptional { get; set; }
    }

    public class TableStateViewModel<T> : TableStateViewModel
    {
        public T? Filter { get; set; }
    }
}
