using Microsoft.AspNetCore.WebUtilities;

namespace WebApplication7
{
    /// <summary>
    /// OOP specific pipeline extensions
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
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
                };
                dto.Errors.Add(new ErrorResponseDto.ErrorModel()
                {
                    Message = ReasonPhrases.GetReasonPhrase(context.HttpContext.Response.StatusCode),
                });

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
                    };
                    dto.Errors.Add(new ErrorResponseDto.ErrorModel()
                    {
                        Message = ReasonPhrases.GetReasonPhrase(context.HttpContext.Response.StatusCode),
                    });

                    await context.HttpContext.Response.WriteAsJsonAsync(dto);
                };
            });

            return services;
        }
    }
}
