using Microsoft.Extensions.DependencyInjection;

namespace Maanfee.Web.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileDownload(this IServiceCollection services)
        {
            services.AddScoped<FileDownloadService>();
            return services;
        }
    }
}
