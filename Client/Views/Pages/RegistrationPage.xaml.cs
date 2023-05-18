using Client.ViewModels.Authorization;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFLibrary.Navigation;

namespace Client.Views.Pages;

public partial class RegistrationPage : Page, IPage<RegistrationViewModel>
{
    public RegistrationPage (RegistrationViewModel viewModel)
    {
        ViewModel = viewModel;
    }

    public RegistrationViewModel ViewModel { get; private init; }

    public void Display ()
    {
        Task.Run(ViewModel.Initialize);

        DataContext = ViewModel;

        InitializeComponent();
    }

    private void passTB_PasswordChanged (object sender, System.Windows.RoutedEventArgs e)
    {
        dangerText.Text = string.Empty;

        passwordPlaceholder.IsEnabled = string.IsNullOrWhiteSpace(passTB.Password);

        ViewModel.Password = passTB.Password;
    }
}
