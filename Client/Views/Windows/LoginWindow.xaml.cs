using Client.ViewModels.Authorization;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Views.Windows;

public partial class LoginWindow : Window
{
    private readonly AuthNavigationViewModel _viewModel;

    public LoginWindow (AuthNavigationViewModel viewModel)
    {
        _viewModel = viewModel;

        InitializeComponent();
        DataContext = _viewModel;

        Task.Run(_viewModel.Initialize);
    }

    private void Minimize_Click (object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void Exit_Click (object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void Window_MouseLeftButtonDown (object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        DragMove();
    }
}