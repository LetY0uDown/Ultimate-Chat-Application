using System.Net;

namespace WPFLibrary.Web;

public record class ApiResponse
{
    public ApiResponse(HttpStatusCode statusCode, string content)
    {
        StatusCode = statusCode;
        Content = content;
    }

    public HttpStatusCode StatusCode { get; private init; }

    public string? Content { get; private init; }
}