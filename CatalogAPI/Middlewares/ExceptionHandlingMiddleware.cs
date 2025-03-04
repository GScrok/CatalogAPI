using System.Text.Json;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IExceptionManager _exceptionManager;

    public ExceptionHandlingMiddleware(RequestDelegate next, IExceptionManager exceptionManager)
    {
        _next = next;
        _exceptionManager = exceptionManager;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var (statusCode, errorResponse) = _exceptionManager.HandleException(ex);

            errorResponse.Details = errorResponse.Details ?? "Detalhes não disponíveis.";

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
