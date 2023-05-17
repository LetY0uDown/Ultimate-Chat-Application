using System.Threading.Tasks;
using WPFLibrary.DI.Attributes;
using WPFLibrary.Navigation;

namespace Client.ViewModels;

[Lifetime(Lifetime.Singleton)]
public sealed class ApplicationNavigationViewModel : ViewModel, INavigationViewModel
{
    private readonly INavigation _navigation;

    public ApplicationNavigationViewModel (INavigation navigation)
    {
        _navigation = navigation;
    }

    public IPage<ViewModel> CurrentPage { get; set; } = null!;

    public override Task Initialize ()
    {
        _navigation.NavigationModel = this;

        return base.Initialize();
    }
}