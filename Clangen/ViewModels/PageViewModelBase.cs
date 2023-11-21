using Avalonia.SimpleRouter;
using Clangen.Models;

namespace Clangen.ViewModels;

public class PageViewModelBase : ViewModelBase
{
    protected HistoryRouter<PageViewModelBase> _router;
    protected Game _game;

    public PageViewModelBase(Game game, HistoryRouter<PageViewModelBase> router)
    {
        _router = router;
        _game = game;
    }
}