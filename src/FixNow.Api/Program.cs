using System.Text.Json.Serialization;

using FixNow.Application;
using FixNow.Infrastructure;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (DbUpdateConcurrencyException)
    {
        // Optimistic concurrency conflict (e.g. two technicians accepting
        // the same service request). Reported as a 409 conflict.
        context.Response.StatusCode = StatusCodes.Status409Conflict;
        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "ConcurrencyConflict",
            Detail = "The resource was modified by another user. Please refresh and try again."
        });
    }
});

var uploadsRoot = Path.Combine(
    app.Environment.ContentRootPath,
    "uploads");

Directory.CreateDirectory(uploadsRoot);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsRoot),
    RequestPath = "/uploads",
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();