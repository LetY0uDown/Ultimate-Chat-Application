using Client.ViewModels.Authorization;
using System.Threading.Tasks;
using System.Windows;
using WPF_Library.Navigation;

namespace Client.Views.Windows;

public partial class LoginWindow : Window, IWindow
{
    private readonly AuthNavigationViewModel _viewModel;

    public LoginWindow (AuthNavigationViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    private void Minimize_Click (object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void Exit_Click (object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    async Task IWindow.Show ()
    {
        await _viewModel.Initialize();

        DataContext = _viewModel;

        InitializeComponent();

        Show();
    }

    Task IWindow.Close ()
    {
        return Task.CompletedTask;
    }
}