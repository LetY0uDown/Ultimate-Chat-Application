using Microsoft.Extensions.DependencyInjection;
using WPFLibrary.Navigation;

namespace Client.Core.Services;

public class PageFactory : IPageFactory
{
    public T CreatePage<T> () where T : IPage<ViewModel>
    {
        return App.Host.Services.GetRequiredService<T>();
    }
}