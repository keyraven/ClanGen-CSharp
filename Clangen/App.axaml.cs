using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.SimpleRouter;
using Clangen.Models;
using Microsoft.Extensions.DependencyInjection;

using Clangen.ViewModels;
using Clangen.Views;

namespace Clangen;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);
        
        IServiceProvider services = ConfigureServices();
        var mainViewModel = services.GetRequiredService<MainViewModel>();

        var game = services.GetRequiredService<Game>();
        game.GameStart();
        Console.WriteLine("Generating Random Clan for Testing... (App.axaml.cs)");
        game.GenerateRandomWorld(); // For Testing
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView()
            {
                DataContext = mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();

        
    }
    
    
    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        //Add game as a service
        services.AddSingleton<Game>();
        
        // Add the HistoryRouter as a service
        services.AddSingleton<HistoryRouter<PageViewModelBase>>(s => new HistoryRouter<PageViewModelBase>(t => (PageViewModelBase)s.GetRequiredService(t)));
        
        // Add the ViewModels as a service (Main as singleton, others as transient)
        services.AddSingleton<MainViewModel>();
        services.AddTransient<ClanScreenViewModel>();
        services.AddTransient<StartScreenViewModel>();
        services.AddTransient<CatProfileScreenViewModel>();
        return services.BuildServiceProvider();
    }
}
