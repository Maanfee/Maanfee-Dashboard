using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Booklet
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBooklet(this IServiceCollection services)
        {
            services.AddScoped<Booklet>();
            return services;
        }
    }
}
