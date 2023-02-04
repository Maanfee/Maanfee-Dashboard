using System.Threading.Tasks;

namespace Maanfee.Web.Printing
{
    public interface IPrintingService
    {
        Task Print();

        Task AddClass();

        Task RemoveClass();
    }
}