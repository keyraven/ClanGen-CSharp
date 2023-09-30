using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;
using Clangen.Views;

namespace Clangen.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _content = default!;

    public MainViewModel(HistoryRouter<ViewModelBase> router)
    {
        // register route changed event to set content to viewModel, whenever 
        // a route changes
        router.CurrentViewModelChanged += viewModel => Content = viewModel;
        
        // change to HomeView 
        router.GoTo<StartScreenViewModel>();
    }
}
