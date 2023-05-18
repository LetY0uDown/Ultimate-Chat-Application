using Client.ViewModels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client.Views.Windows;

public partial class ApplicationWindow : Window
{
    private readonly ApplicationNavigationViewModel _viewModel;

    public ApplicationWindow (ApplicationNavigationViewModel viewModel)
    {
        _viewModel = viewModel;

        InitializeComponent();

        Task.Run(_viewModel.Initialize);

        DataContext = _viewModel;
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
}