using WPFLibrary.Web.Interfaces;

namespace WPFLibrary.Web;

public class RequestBuilderOptions
{
    public IHTTPExceptionHandler? ExceptionHandler { get; set; }

    public string? JWTToken { get; set; }

    public string? BaseURL { get; set; }
}