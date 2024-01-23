using Microsoft.Extensions.DependencyInjection;

namespace Maanfee.Web.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileDownload(this IServiceCollection services)
        {
            services.AddSingleton<FileDownloadService>();
            return services;
        }
    }
}
