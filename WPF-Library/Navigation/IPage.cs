using System.Threading.Tasks;

namespace WPFLibrary.Navigation;

public interface IPage<out TViewModel> where TViewModel : ViewModel
{
    TViewModel ViewModel { get; }

    Task Display ();

    Task Leave ();
}