using Models;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using WPFLibrary;
using WPFLibrary.DI.Attributes;
using WPFLibrary.Navigation;
using WPFLibrary.Web.Interfaces;

namespace Client.ViewModels.Authorization;

[Lifetime(Lifetime.Transient)]
public sealed class RegistrationViewModel : ViewModel
{
    private readonly INavigation _navigation;
    private readonly IApiRequestBuilder _requestBuilder;

    public RegistrationViewModel (INavigation navigation, IApiRequestBuilder requestBuilder)
    {
        _navigation = navigation;
        _requestBuilder = requestBuilder;

        RegistrationCommand = new(async o => {
            DangerText = string.Empty;

            var user = new User {
                ID = string.Empty,
                Username = Username,
                Password = Password,
                Email = Email
            };

            var response = await _requestBuilder.CreateRequest("Register")
                                                .AddJsonBody(user)
                                                .ExecuteAsync(HttpMethod.Post);

            if (response.StatusCode != HttpStatusCode.OK) {
                DangerText = response.Content?.Replace("\"", "")!;
                return;
            }

            App.AuthorizeData = JsonSerializer.Deserialize<AuthorizeData>(response.Content)!;

            //await _navigation.SetMainWindow<ApplicationWindow>();
        }, b => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email));

        RedirectToLoginCommand = new(o => {
            _navigation.DisplayPage<IPage<LoginViewModel>>();
        });
    }

    public UICommand RegistrationCommand { get; private init; }
    public UICommand RedirectToLoginCommand { get; private init; }

    public string DangerText { get; private set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }
}