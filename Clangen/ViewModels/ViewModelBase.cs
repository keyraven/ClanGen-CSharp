using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;

namespace Clangen.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected HistoryRouter<ViewModelBase> _router;

    public ViewModelBase(HistoryRouter<ViewModelBase> router)
    {
        _router = router;
    }
}
