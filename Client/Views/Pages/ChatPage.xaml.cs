using Client.ViewModels.Messanger;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFLibrary.Navigation;

namespace Client.Views.Pages;

public partial class ChatPage : Page, IPage<ChatViewModel>
{
    public ChatPage (ChatViewModel viewModel)
    {
        ViewModel = viewModel;
    }

    public ChatViewModel ViewModel { get; private init; }

    public async Task Display ()
    {
        await ViewModel.Initialize();
        DataContext = ViewModel;

        InitializeComponent();
    }

    public async Task Leave ()
    {
        await ViewModel.Disable();
    }
}
