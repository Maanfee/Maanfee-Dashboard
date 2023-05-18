using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);

// ********************* Ocelot *********************
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
// **************************************************

// Fixed Self referencing loop detected with type and CaseInsensitive
builder.Services.AddControllers()
	.AddNewtonsoftJson(options =>
	{
		options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
	})
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
	});

var app = builder.Build();

app.UseCors(builder =>
{
	//builder.WithOrigins("https://localhost:4030");
	builder.AllowAnyHeader();
	builder.AllowAnyMethod();
	builder.AllowAnyOrigin();
});
app.UseRouting(); 

app.UseAuthentication();
app.UseAuthorization();

//app.MapGet("/", () => "Hello World!");
app.UseEndpoints(endpoints =>
{
	endpoints.MapGet("/",
		async context => { await context.Response.WriteAsync("Ocelot API Gateway"); });
});
//app.UseEndpoints(endpoints => {
//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Ping}");
//});
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}"
//);

// ********************* Ocelot *********************
app.UseOcelot().Wait();
// **************************************************

app.Run();
