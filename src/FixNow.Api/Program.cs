using System.Text.Json.Serialization;

using FixNow.Application;
using FixNow.Infrastructure;

using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });


var app = builder.Build();

var uploadsRoot = Path.Combine(
    app.Environment.ContentRootPath,
    "uploads");

Directory.CreateDirectory(uploadsRoot);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsRoot),
    RequestPath = "/uploads",
});

app.MapControllers();

app.Run();