using Microsoft.Extensions.DependencyInjection;

namespace Maanfee.Web.JSInterop
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsQuery(this IServiceCollection services)
        {
            services.AddTransient<Dom>();
			services.AddTransient<LocalStorage>();
            services.AddTransient<Fullscreen>();
            services.AddTransient<Screen>();
            services.AddTransient<History>();

            return services;
        }
    } 
}
