using Client.Core;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WPFLibrary.DI.Attributes;
using WPFLibrary.Navigation;
using WPFLibrary.Web.Interfaces;

namespace Client.ViewModels.Authorization;

[Lifetime(Lifetime.Singleton)]
public sealed class AuthNavigationViewModel : ViewModel, INavigationViewModel
{
    private readonly INavigation _navigation;
    private readonly IApiRequestBuilder _requestBuilder;
    private readonly IConfiguration _configuration;

    public AuthNavigationViewModel (INavigation navigation, IApiRequestBuilder requestBuilder, IConfiguration configuration)
    {
        _navigation = navigation;
        _requestBuilder = requestBuilder;
        _configuration = configuration;
    }

    public IPage<ViewModel> CurrentPage { get; set; } = null!;

    public override async Task Initialize ()
    {
        _requestBuilder.ConfigureOptions(options => {
            options.BaseURL = _configuration["BaseURL"];
            options.ExceptionHandler = new RequestErrorHandler();
        });

        _navigation.NavigationModel = this;

        await _navigation.DisplayPage<IPage<LoginViewModel>>();
    }
}