using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Services;
using Maanfee.Dashboard.Services.Extensions;
using Maanfee.Logging.Console;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;

namespace Maanfee.Dashboard.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
			services.AddSignalR();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            // RealTime Logging
            services.AddLoggingConsole(new Uri("http://localhost:22001"), new Uri("http://172.17.17.22"));

            #region - Internal Services -

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            services.AddServerServices(Configuration, "SQLServerConnection_DebugMode");

            // ********************************************
            services.AddSwaggerServices(Configuration);
            // ********************************************

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
            app.UseResponseCompression();
            app.UseStaticFiles();//
			try
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "D:\\_UploadedDocuments")),
                    RequestPath = "/StaticFiles"
                });
            }
            catch
            {
               
            }

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
            app.UseResponseCompression();   //
            app.UseAntiforgery();

            #region - Internal Configuration -

            //LocalConfigurationService.InitServerCultureAsync(app);
            app.UseRequestLocalization(options =>
            {
                var supportedCultures = new[] { "en-US", "fa-IR" };
                options.SetDefaultCulture(supportedCultures[1])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
            });
            app.AddConfigure(env, SQLServerContext, SQLiteContext);

            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
                endpoints.MapHub<LoggingHub>("/LoggingHub");
            });
        }

        // ********************************

    }
}
