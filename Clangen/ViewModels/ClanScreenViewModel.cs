using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;
using Avalonia.Media.Imaging;
using Clangen.Models;

namespace Clangen.ViewModels;

public partial class ClanScreenViewModel : ViewModelBase
{
    [ObservableProperty] 
    private Bitmap? _catImage;
    
    public ClanScreenViewModel(HistoryRouter<ViewModelBase> router) : base(router)
    {
        
    }
}