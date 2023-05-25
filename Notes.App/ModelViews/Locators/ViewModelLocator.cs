using Microsoft.Extensions.DependencyInjection;

namespace Notes.App.ModelViews.Locators
{
    internal class ViewModelLocator
    {
        public static MainWindowViewModel mainWindowModel => App
            .host
            .Services
            .GetRequiredService<MainWindowViewModel>();
    }
}
