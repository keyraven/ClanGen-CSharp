using Avalonia.SimpleRouter;
using Clangen.Models;

namespace Clangen.ViewModels;

public class MainViewModelBase : ViewModelBase
{
    protected HistoryRouter<ViewModelBase> _router;
    protected Game _game;

    public MainViewModelBase(Game game, HistoryRouter<ViewModelBase> router)
    {
        _router = router;
        _game = game;
    }
}