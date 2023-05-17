namespace WPFLibrary.Navigation;

public interface IPageFactory
{
    T CreatePage<T> () where T : IPage<ViewModel>;
}