using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WPFLibrary.Web.Interfaces;

public interface IApiRequestBuilder
{
    void ConfigureOptions(Action<RequestBuilderOptions> action);

    IApiRequestBuilder CreateRequest(string url);

    IApiRequestBuilder AddJsonBody<T>(T obj) where T : class;

    IApiRequestBuilder AddFile(string path);

    IApiRequestBuilder UseAuthentication();

    Task<ApiResponse> ExecuteAsync(HttpMethod method);
}