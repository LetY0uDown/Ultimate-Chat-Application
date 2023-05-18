using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WPFLibrary.Navigation;

namespace Client.Core.Services;

public class PageFactory : IPageFactory
{
    public T CreatePage<T> () where T : IPage<ViewModel>
    {
        T page = default;

        Application.Current.Dispatcher.Invoke(() => page = App.Host.Services.GetRequiredService<T>());
        return page;
    }
}