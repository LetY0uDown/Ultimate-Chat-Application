using System;
using System.Collections.Generic;
using WPFLibrary.Navigation;

namespace Client.Core.Services;

internal class Navigation : INavigation
{
    private readonly List<IPage<ViewModel>> _pages = new();
    private int _pageIndex = 0;

    private readonly IPageFactory _pageFactory;
    private INavigationViewModel _navigationModel = null!;

    public Navigation (IPageFactory pageFactory)
    {
        _pageFactory = pageFactory;
    }

    public INavigationViewModel NavigationModel
    {
        get => _navigationModel;
        set {
            _navigationModel = value;
            _pages.Clear();
        }
    }

    public void DisplayPage<T> (params (string Title, object Value)[] parameters) where T : IPage<ViewModel>
    {
        throw new NotImplementedException();
    }

    public void DisplayPage<T> () where T : IPage<ViewModel>
    {
        var page = _pageFactory.CreatePage<T>();

        if (_pageIndex < _pages.Count - 1) {
            _pages.RemoveRange(_pageIndex + 1, _pages.Count - _pageIndex - 1);
        }

        _pages.Add(page);
        _pageIndex = _pages.Count - 1;

        SetPage(_pageIndex);
    }

    public void DisplayNext ()
    {
        if (_pageIndex >= _pages.Count - 1)
            return;

        SetPage(++_pageIndex);
    }

    public void DisplayPrevious ()
    {
        if (_pageIndex <= 0)
            return;

        SetPage(--_pageIndex);
    }

    private void SetPage (int index)
    {
        NavigationModel?.CurrentPage?.Leave();

        NavigationModel.CurrentPage = _pages[index];
        NavigationModel.CurrentPage.Display();
    }
}