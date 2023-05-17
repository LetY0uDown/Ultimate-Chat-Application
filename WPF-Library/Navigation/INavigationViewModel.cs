namespace WPFLibrary.Navigation;

public interface INavigationViewModel
{
    IPage<ViewModel> CurrentPage { get; set; }
}