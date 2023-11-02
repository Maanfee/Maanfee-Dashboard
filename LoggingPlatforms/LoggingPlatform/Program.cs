using LoggingPlatform.Extensions;
using Maanfee.Logging.Console;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<JwtHelpers>();
builder.Services.AddJwtTokenServices(builder.Configuration);
builder.Services.AddSwaggerServices(builder.Configuration);

builder.Services.AddScoped<HttpClient>();

// Fixed Self referencing loop detected with type and CaseInsensitive
builder.Services.AddControllers()
	.AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; })
	.AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNameCaseInsensitive = false; });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() /*|| app.Environment.IsProduction()*/)
{
	app.UseDeveloperExceptionPage();
	//app.UseSwagger();
	//app.UseSwaggerUI(/*c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor.Server v1")*/);
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

app.UseCors(builder =>
{
	//builder.WithOrigins("https://localhost:44379");
	builder.AllowAnyHeader();
	builder.AllowAnyMethod();
	builder.AllowAnyOrigin();
}); 
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#if DEBUG

//var ContextOptions = new DbContextOptionsBuilder<_BaseContext_SQLServer>()
//	.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection_DebugMode")).Options;
//var SQLServerContext = new _BaseContext_SQLServer(ContextOptions);

//DbInitializer<_BaseContext_SQLServer>.DatabaseCreating(SQLServerContext);
//SQLServerDataInitializer.Initialize(SQLServerContext);

#endif

app.Run();
