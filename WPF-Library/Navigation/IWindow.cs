using System.Threading.Tasks;

namespace WPF_Library.Navigation;

public interface IWindow
{
    Task Show ();

    Task Close ();
}