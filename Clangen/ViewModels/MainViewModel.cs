using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;
using Clangen.Models;
using Clangen.Views;

namespace Clangen.ViewModels;

public partial class MainViewModel : MainViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _content = default!;

    public MainViewModel(Game game, HistoryRouter<ViewModelBase> router) : base(game, router)
    {
        // register route changed event to set content to viewModel, whenever 
        // a route changes
        _router.CurrentViewModelChanged += viewModel => Content = viewModel;
        
        // change to HomeView 
        _router.GoTo<StartScreenViewModel>();
        
    }
}
