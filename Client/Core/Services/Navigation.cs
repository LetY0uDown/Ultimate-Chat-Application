using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public Task DisplayPage<T> (params (string Title, object Value)[] parameters) where T : IPage<ViewModel>
    {
        throw new NotImplementedException();
    }

    public async Task DisplayPage<T> () where T : IPage<ViewModel>
    {
        var page = _pageFactory.CreatePage<T>();

        if (_pageIndex < _pages.Count - 1) {
            _pages.RemoveRange(_pageIndex + 1, _pages.Count - _pageIndex - 1);
        }

        _pages.Add(page);
        _pageIndex = _pages.Count - 1;

        await SetPage(_pageIndex);
    }

    public async Task DisplayNext ()
    {
        if (_pageIndex >= _pages.Count - 1)
            return;

        await SetPage(++_pageIndex);
    }

    public async Task DisplayPrevious ()
    {
        if (_pageIndex <= 0)
            return;

        await SetPage(--_pageIndex);
    }

    private async Task SetPage (int index)
    {
        if (NavigationModel.CurrentPage is not null)
            await NavigationModel.CurrentPage.Leave();

        NavigationModel.CurrentPage = _pages[index];
        await NavigationModel.CurrentPage.Display();
    }
}