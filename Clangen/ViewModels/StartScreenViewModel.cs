using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;

namespace Clangen.ViewModels;

public partial class StartScreenViewModel : ViewModelBase
{
    public string Greeting => "Start Screen!";
    
    public StartScreenViewModel(HistoryRouter<ViewModelBase> router) : base(router)
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
