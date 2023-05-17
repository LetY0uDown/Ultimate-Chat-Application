using Client.ViewModels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Library.Navigation;

namespace Client.Views.Windows;

public partial class ApplicationWindow : Window, IWindow
{
    private readonly ApplicationNavigationViewModel _viewModel;

    public ApplicationWindow (ApplicationNavigationViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    private void Border_MouseLeftButtonDown (object sender, MouseButtonEventArgs e)
    {
        DragMove();
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

    async Task IWindow.Close ()
    {
        await _viewModel.Disable();

        Close ();
    }
}