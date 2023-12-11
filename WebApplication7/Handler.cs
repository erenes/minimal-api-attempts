using Microsoft.AspNetCore.Diagnostics;

internal class Handler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        /*
        var dto = new ErrorResponseDto()
        {
            Status = httpContext.Response.StatusCode,
        };
        dto.Errors.Add(new ErrorResponseDto.ErrorModel()
        {
            Message = ReasonPhrases.GetReasonPhrase(httpContext.Response.StatusCode),
        });

        await httpContext.Response.WriteAsJsonAsync(dto);
        return true;
        */

        return ValueTask.FromResult(false);
    }
}