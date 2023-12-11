using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApplication7;
using WebApplication7.Endpoints;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();

ConfigurePipeline(app);

app.Run();

static void ConfigureServices(WebApplicationBuilder builder)
{
    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddExceptionHandler<Handler>();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
         {
             c.Audience = "https://oyster";
         });

    builder.Services.AddAuthorization();
    builder.Services.Configure<RouteHandlerOptions>(options =>
    {
        options.ThrowOnBadRequest = true;
    });

    builder.Services.AddProblemDetails();
}

static void ConfigurePipeline(WebApplication app)
{
    app.UseExceptionHandler(new ExceptionHandlerOptions()
    {
        AllowStatusCode404Response = true,
    });

    app.Use(async (context, next) =>
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            throw;
        }
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseStatusCodePageWithErrorMessageDto();

    app.UseAuthentication();
    app.UseAuthorization();

    var global = app
        .MapGroup("/api")
        .WithOpenApi();
    // .RequireAuthorization();

    WeatherEndpoints.RegisterEndpoints(global);

    var tm = typeof(WeatherEndpoints).GetProperties();

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