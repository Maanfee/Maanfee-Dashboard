using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Server.Data;
using Maanfee.Dashboard.Services;
using Maanfee.Dashboard.Services.Extensions;
using Maanfee.Dashboard.Views.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Maanfee.Dashboard.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
			services.AddSignalR();

			#region - Internal Services -

			services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            services.AddServerServices(Configuration, "SQLServerConnection_DebugMode");

            // ***********************************
            services.AddSwaggerServices(Configuration);
            // *******************************

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, _BaseContext_SQLServer SQLServerContext
           , _BaseContext_SQLite SQLiteContext, _BaseContext_InMemory InMemoryContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseBlazorFrameworkFiles();  //
            app.UseStaticFiles();//
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "D:\\_UploadedDocuments")),
                RequestPath = "/StaticFiles"
            });

            // *************************** Swagger ***************************
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", typeof(Program).Assembly.GetName().Name);
                options.RoutePrefix = "swagger";
                options.DisplayRequestDuration();
            });
            // ***************************************************************

            app.UseRouting();  //

            #region - Internal Configuration -

            LocalConfigurationService.InitServerCultureAsync(app);
            app.AddConfigure(env, SQLServerContext, SQLiteContext);

            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
				endpoints.MapHub<SignalRHub>("/signalRHub");
			});
        }

        // ********************************

    }
}
