using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WPFLibrary.Web;
using WPFLibrary.Web.Interfaces;

namespace Client.Core.Services;

public class RestRequestBuilder : IApiRequestBuilder
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReferenceHandler = ReferenceHandler.Preserve,
        MaxDepth = 0
    };

    private readonly RestClientOptions _options = new();

    private RestRequest _request = null!;

    public RequestBuilderOptions _builderOptions = new();

    public void ConfigureOptions(Action<RequestBuilderOptions> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        action.Invoke(_builderOptions);

        ArgumentNullException.ThrowIfNull(_builderOptions.BaseURL, "BaseURL");
    }

    public IApiRequestBuilder CreateRequest(string url)
    {
        _request = new RestRequest(url);

        return this;
    }

    public IApiRequestBuilder AddFile(string path)
    {
        throw new NotImplementedException();
    }

    public IApiRequestBuilder AddJsonBody<T>(T obj) where T : class
    {
        ArgumentNullException.ThrowIfNull(obj);

        if (_request is null)
            throw new NullReferenceException("Request is not created yet");

        _request.AddJsonBody(obj);

        return this;
    }

    public IApiRequestBuilder UseAuthentication()
    {
        if (!string.IsNullOrWhiteSpace(_builderOptions.JWTToken))
        {
            _options.Authenticator = new JwtAuthenticator(_builderOptions.JWTToken);
        }

        return this;
    }

    public async Task<ApiResponse> ExecuteAsync(HttpMethod method)
    {
        ApiResponse response = null!;

        _request.Method = DetermineMethod(method);

        _options.BaseUrl = new(_builderOptions?.BaseURL!);

        using (RestClient client = new(_options,
                                    configureSerialization: s => s.UseSystemTextJson(_jsonOptions)))
        {
            try
            {
                var resp = await client.ExecuteAsync(_request);
                response = new(resp.StatusCode, resp.Content!);
            }
            catch (Exception ex)
            {
                _builderOptions?.ExceptionHandler?.HandleException(ex);
            }
        }

        return response;
    }

    private static Method DetermineMethod(HttpMethod method)
    {
        return method.ToString() switch
        {
            "GET" => Method.Get,
            "POST" => Method.Post,
            "PUT" => Method.Put,
            "DELETE" => Method.Delete,
            _ => throw new NotImplementedException()
        };
    }
}