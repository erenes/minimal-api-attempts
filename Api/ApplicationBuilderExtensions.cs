using Microsoft.AspNetCore.WebUtilities;

namespace Api;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseExceptions(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
         {
             try
             {
                 await next(context);
             }
             catch (Exception ex)
             {
                 context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                 await context.Response.WriteAsJsonAsync(new ErrorResponseDto()
                 {
                     Status = StatusCodes.Status500InternalServerError,
                     Errors =
                     [
                         new()
                         {
                            Message = ex.Message,
                         },
                    ],
                 });
             }
         });
    }

    /// <summary>
    /// Configured the IApplicationBuilder to use the standard UseStatusCodePages but with a body according to ErrorResponseDto
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <returns>Returns the application builder</returns>
    public static IApplicationBuilder UseStatusCodePageWithErrorMessageDto(this IApplicationBuilder app)
    {
        app.UseStatusCodePages(async context =>
        {
            var dto = new ErrorResponseDto()
            {
                Status = context.HttpContext.Response.StatusCode,
                Errors =
                [
                    new()
                    {
                        Message = ReasonPhrases.GetReasonPhrase(context.HttpContext.Response.StatusCode),
                    }
                ]
            };

            await context.HttpContext.Response.WriteAsJsonAsync(dto);
        });

        return app;
    }

    public static IServiceCollection AddProblemDetailsWithErrorMessageDto(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = async (context) =>
            {
                var dto = new ErrorResponseDto()
                {
                    Status = context.HttpContext.Response.StatusCode,
                    Errors =
                    [
                        new()
                        {
                            Message = ReasonPhrases.GetReasonPhrase(context.HttpContext.Response.StatusCode),
                        }
                    ]
                };

                await context.HttpContext.Response.WriteAsJsonAsync(dto);
            };
        });

        return services;
    }
}
