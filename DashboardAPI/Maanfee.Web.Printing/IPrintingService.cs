using System.Threading.Tasks;

namespace Maanfee.Web.Printing
{
    public interface IPrintingService
    {
        Task PrintAsync(bool IsBackward = true);

        Task AddClassAsync();

        Task RemoveClassAsync();
    }
}