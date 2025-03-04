using Microsoft.AspNetCore.Http;

public class ErrorResponse
{
    public string Message { get; set; }
    public int Status { get; set; }
    public string? Details { get; set; }

    public ErrorResponse(string message, int statusCode, string? details = null)
    {
        Message = message;
        Status = statusCode;
        Details = details;
    }
}