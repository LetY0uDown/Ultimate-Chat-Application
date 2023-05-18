using System.Threading.Tasks;

namespace WPFLibrary.Navigation;

public interface INavigation
{
    INavigationViewModel NavigationModel { get; set; }

    void DisplayPage<T> () where T : IPage<ViewModel>;
    void DisplayPage<T> (params (string Title, object Value)[] parameters) where T : IPage<ViewModel>;

    void DisplayNext ();
    void DisplayPrevious ();
}