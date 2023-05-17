using System.Threading.Tasks;

namespace WPFLibrary.Navigation;

public abstract class ViewModel : ObservableObject
{
    public virtual Task Initialize ()
    {
        return Task.CompletedTask;
    }

    public virtual Task Disable ()
    {
        return Task.CompletedTask;
    }
}