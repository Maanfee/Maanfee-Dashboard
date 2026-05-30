using Newtonsoft.Json;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ********************* Ocelot *********************
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
//builder.Configuration.AddJsonFile("ocelot.xxx.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);
// **************************************************

// Fixed Self referencing loop detected with type
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
);

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

// app.MapGet("/", () => "Ocelot API Gateway");
//app.MapGet("/", async context => await context.Response.WriteAsync("Ocelot API Gateway"));

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        await context.Response.WriteAsync("Ocelot API Gateway");
        return;
    }
    await next();
});

// ********************* Ocelot *********************
await app.UseOcelot();
// **************************************************

app.Run();
