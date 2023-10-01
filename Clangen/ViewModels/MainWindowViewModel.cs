using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;

namespace Clangen.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(HistoryRouter<ViewModelBase> router) : base(router)
    {
        
    }
}