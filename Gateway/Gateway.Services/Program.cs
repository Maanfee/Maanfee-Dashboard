using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ********************* Ocelot *********************
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
// **************************************************

var app = builder.Build();

app.UseRouting();
app.MapGet("/", () => "Hello World!");
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});
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
