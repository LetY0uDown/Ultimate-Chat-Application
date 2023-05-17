using Client.Core.Services;
using Client.ViewModels.Authorization;
using Client.Views.Pages;
using Client.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using WPF_Library.Navigation;
using WPFLibrary.DI.Extensions;
using WPFLibrary.Navigation;
using WPFLibrary.Web.Interfaces;

namespace Client;

public record class AuthorizeData (string ID, string Token);

public partial class App : Application
{
    internal static AuthorizeData AuthorizeData { get; set; } = null!;

    internal static IHost Host { get; private set; } = null!;

    private async void Application_Startup (object sender, StartupEventArgs e)
    {
        Host = ConfigureHosting();

        IWindow loginWindow = new LoginWindow(Host.Services.GetService<AuthNavigationViewModel>()!);
        await loginWindow.Show();
    }

    private static IHost ConfigureHosting ()
    {
        var builder = new ConfigurationBuilder()
                          .SetBasePath("C:\\Users\\maksm\\source\\repos\\Ultimate Chat Application\\Client\\Resources\\")
                          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        var config = builder.Build();

        var hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder().ConfigureServices(services => {
            services.AddSingleton(typeof(IConfiguration), config);

            services.RegisterViewModels<ViewModel>();

            services.AddSingleton<IPageFactory, PageFactory>();
            services.AddSingleton<INavigation, Navigation>();
            services.AddSingleton<IApiRequestBuilder, RestRequestBuilder>();

            services.AddTransient<IPage<LoginViewModel>, LoginPage>();
            services.AddTransient<IPage<RegistrationViewModel>, RegistrationPage>();
        });

        return hostBuilder.Build();
    }
}