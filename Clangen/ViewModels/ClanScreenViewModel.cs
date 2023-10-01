namespace Clangen.ViewModels;
using Avalonia.SimpleRouter;

public partial class ClanScreenViewModel : ViewModelBase
{
    public string Greeting => "Clan Screen!";

    public ClanScreenViewModel(HistoryRouter<ViewModelBase> router) : base(router)
    {
        
    }
}