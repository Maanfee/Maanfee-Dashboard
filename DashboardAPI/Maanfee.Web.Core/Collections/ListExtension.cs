using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maanfee.Web.Core
{
    public static class ListExtension
    {
        public static int PageIndex { get; private set; }

        public static int TotalPages<T>(this IList<T> list)
        {
            return list.Count();
        }

        public static async Task<List<T>> ToPaginateAsync<T>(this IQueryable<T> list, int pageIndex, int pageSize)
        {
            TotalPages<T>(await list.ToListAsync());
            //var count = await list.CountAsync();

            //PageIndex = pageIndex;
            //TotalPages((int)Math.Ceiling(count / (double)pageSize));

            var items = await list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new List<T>(items);
        }
    }
}
