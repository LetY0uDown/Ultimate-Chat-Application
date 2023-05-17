using Client.ViewModels.Authorization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFLibrary.Navigation;

namespace Client.Views.Pages;

public partial class LoginPage : Page, IPage<LoginViewModel>
{
    public LoginPage (LoginViewModel viewModel)
    {
        ViewModel = viewModel;
    }

    public LoginViewModel ViewModel { get; private init; }

    public async Task Display ()
    {
        await ViewModel.Initialize();

        DataContext = ViewModel;

        InitializeComponent();
    }

    public Task Leave ()
    {
        return Task.CompletedTask;
    }

    private void passTB_PasswordChanged (object sender, RoutedEventArgs e)
    {
        passwordPlaceholder.IsEnabled = string.IsNullOrWhiteSpace(passTB.Password);

        ViewModel.Password = passTB.Password;
    }
}