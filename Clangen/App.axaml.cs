using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Clangen.Models;
using Clangen.RegionAdapters;
using Clangen.ViewModels;
using Clangen.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;

namespace Clangen;

public class App : PrismApplication
{
    public static bool IsSingleViewLifetime =>
        Environment.GetCommandLineArgs()
            .Any(a => a == "--fbdev" || a == "--drm");
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        base.Initialize();              // <-- Required
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // Register Services
        //TODO - make Game an interface (IGame)
        containerRegistry.RegisterSingleton<Game,Game>();

        // Views - Generic
        containerRegistry.Register<MainWindow>();
        containerRegistry.Register<MainWindow>();

        // Views - Region Navigation
        containerRegistry.RegisterForNavigation<StartScreenView, StartScreenViewModel>();
        containerRegistry.RegisterForNavigation<ClanScreenView, ClanScreenViewModel>();
    }

    
    protected override AvaloniaObject CreateShell()
    { 
        return Container.Resolve<MainWindow>();
    }


    protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
    {
        base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        regionAdapterMappings.RegisterMapping(typeof(Grid), Container.Resolve<GridRegionAdapter>());
    }
    
    /*

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        // Register modules
        //moduleCatalog.AddModule<Module1.Module>();
        //moduleCatalog.AddModule<Module2.Module>();
        //moduleCatalog.AddModule<Module3.Module>();
    }
    */

    /// <summary>Called after <seealso cref="Initialize"/>.</summary>
    protected override void OnInitialized()
    {
        // Register initial Views to Region.
        var regionManager = Container.Resolve<IRegionManager>();
        regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(StartScreenViewModel));
        regionManager.RegisterViewWithRegion(RegionNames.TestRegion1, typeof(Test1ViewModel));
        regionManager.RegisterViewWithRegion(RegionNames.TestRegion2, typeof(Test2ViewModel));
    }
}
    
