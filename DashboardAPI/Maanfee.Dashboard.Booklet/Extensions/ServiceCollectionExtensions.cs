using Microsoft.Extensions.DependencyInjection;

namespace Maanfee.Dashboard.Views.Booklet
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBooklet(this IServiceCollection services)
        {
            services.AddSingleton<Booklet>();
            return services;
        }
    }
}
