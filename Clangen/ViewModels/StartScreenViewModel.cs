using System;
using Clangen.Models;
using Prism.Regions;
using ReactiveUI;
using System.Windows.Input;

namespace Clangen.ViewModels;

public class StartScreenViewModel : ViewModelBase
{
    private IRegionManager _regionManager;
    private Game _game;
    public string Greeting => "Start Screen!";
    
    public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> PressStartCommand { get; private set; }
    
    
    public StartScreenViewModel(Game game, IRegionManager regionManager)
    {
        _regionManager = regionManager;
        _game = game;

        PressStartCommand = ReactiveCommand.Create(PressStart);
    }
    
    public void PressStart()
    {
        Console.WriteLine("Press Start");

        NavigationParameters parameters = new NavigationParameters();
        parameters.Add("test", "weeeee");
       _regionManager.RequestNavigate(RegionNames.MainRegion, "ClanScreenView", parameters);
    }

    //[RelayCommand]
    public void PressSettings()
    {
        //_router.GoTo<ClanScreenViewModel>();
    }
}
