using System.Threading.Tasks;

namespace WPFLibrary.Navigation;

public interface INavigation
{
    INavigationViewModel NavigationModel { get; set; }

    Task DisplayPage<T> () where T : IPage<ViewModel>;

    Task DisplayPage<T> (params (string Title, object Value)[] parameters) where T : IPage<ViewModel>;

    Task DisplayNext ();

    Task DisplayPrevious ();
}