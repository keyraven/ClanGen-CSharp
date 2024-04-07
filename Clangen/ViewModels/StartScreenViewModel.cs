using Clangen.Models;

namespace Clangen.ViewModels;

public class StartScreenViewModel : PageViewModelBase
{
    public string Greeting => "Start Screen!";
    
    public StartScreenViewModel() 
    {
    }
    
    //[RelayCommand]
    public void PressStart()
    {
       // _router.GoTo<ClanScreenViewModel>();
    }

    //[RelayCommand]
    public void PressSettings()
    {
        //_router.GoTo<ClanScreenViewModel>();
    }
}
