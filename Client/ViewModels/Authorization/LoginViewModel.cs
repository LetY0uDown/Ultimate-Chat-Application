using Client.Core;
using Models;
using System.Net;
using System.Net.Http;
using WPFLibrary;
using WPFLibrary.DI.Attributes;
using WPFLibrary.Navigation;
using WPFLibrary.Web.Interfaces;

namespace Client.ViewModels.Authorization;

[Lifetime(Lifetime.Transient)]
public sealed class LoginViewModel : ViewModel
{
    private readonly INavigation _navigation;
    private readonly IApiRequestBuilder _requestBuilder;

    public LoginViewModel (INavigation navigation, IApiRequestBuilder requestBuilder)
    {
        _navigation = navigation;
        _requestBuilder = requestBuilder;

        LoginCommand = new(async o => {
            DangerText = string.Empty;

            var user = new User {
                ID = string.Empty,
                Username = Username,
                Password = Password,
                Email = string.Empty
            };

            var response = await requestBuilder.CreateRequest("Login")
                                .AddJsonBody(user)
                                .ExecuteAsync(HttpMethod.Get);

            if (response.StatusCode != HttpStatusCode.OK) {
                DangerText = response.Content?.Replace("\"", "");
                return;
            }

            App.AuthorizeData = Serializer.Deserialize<AuthorizeData>(response.Content);

            //await _navigation.SetMainWindow<ApplicationWindow>();
        }, b => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password));

        RedirectToRegistrationCommand = new(o => {
            _navigation.DisplayPage<IPage<RegistrationViewModel>>();
        });
    }

    public UICommand LoginCommand { get; private init; }

    public UICommand RedirectToRegistrationCommand { get; private init; }

    public string? DangerText { get; private set; }

    public string? Username { get; set; }

    public string? Password { get; set; }
}