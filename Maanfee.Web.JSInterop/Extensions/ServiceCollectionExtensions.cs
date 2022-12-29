using Microsoft.Extensions.DependencyInjection;

namespace Maanfee.Web.JSInterop
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsQuery(this IServiceCollection services)
        {
            services.AddScoped<Dom>();
			services.AddScoped<LocalStorage>();
			return services;
        }
    } 
}
