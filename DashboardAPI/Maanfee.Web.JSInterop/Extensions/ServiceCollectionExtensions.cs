using Microsoft.Extensions.DependencyInjection;

namespace Maanfee.Web.JSInterop
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsQuery(this IServiceCollection services)
        {
            services.AddSingleton<Dom>();
			services.AddSingleton<LocalStorage>();
            services.AddSingleton<Fullscreen>();
            services.AddSingleton<Screen>();
            services.AddSingleton<History>();

            return services;
        }
    } 
}
