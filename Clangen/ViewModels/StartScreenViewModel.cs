using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;
using Clangen.Models;

namespace Clangen.ViewModels;

public partial class StartScreenViewModel : MainViewModelBase
{
    public string Greeting => "Start Screen!";
    
    public StartScreenViewModel(Game game, HistoryRouter<ViewModelBase> router) : base(game, router)
    {
        _router = router;
    }
    
    [RelayCommand]
    public void PressStart()
    {
        _router.GoTo<ClanScreenViewModel>();
    }

    [RelayCommand]
    public void PressSettings()
    {
        _router.GoTo<ClanScreenViewModel>();
    }
}
