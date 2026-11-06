using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GerenciamentoEstoque.Services;
using GerenciamentoEstoque.ViewModels;
using GerenciamentoEstoque.Views;

namespace GerenciamentoEstoque;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = new MainWindow();
            var dialogService = new DialogService(() => mainWindow);
            mainWindow.DataContext = new MainViewModel(new ApiClient(), dialogService);
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}