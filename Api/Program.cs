using System.Text.Json.Serialization;
using Api.Endpoints.WeatherForecast;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();

        ConfigurePipeline(app);

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(o =>
        {
            o.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        builder.Services.Configure<RouteHandlerOptions>(options =>
        {
            options.ThrowOnBadRequest = true;
        });

        builder.Services.AddProblemDetailsWithErrorMessageDto();
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        app.UseExceptions();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseStatusCodePageWithErrorMessageDto();

        var global = app
            .MapGroup("/api")
            .WithOpenApi();

        WeatherEndpoints.RegisterEndpoints(global);

        app.MapWhen(context => context.Request.Method == HttpMethod.Get.Method && !context.Request.Path.StartsWithSegments("/api") && !context.Request.Path.StartsWithSegments("/connect"), appServer =>
        {
            appServer.UseSpa(spa =>
            {
                spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions
                {
                    OnPrepareResponse = context =>
                    {
                        // never cache index.html
                        if (context.File.Name == "index.html")
                        {
                            context.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
                            context.Context.Response.Headers.Append("Pragma", "no-cache");
                            context.Context.Response.Headers.Append("Expires", "0");
                        }
                    },
                };
            });
        });
    }
}