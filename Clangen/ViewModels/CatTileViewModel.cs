using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Clangen.ViewModels;

public partial class CatTileViewModel : ViewModelBase
{
    [ObservableProperty] 
    private Bitmap _catImage;

    [ObservableProperty] 
    private string _catName;
    
    public CatTileViewModel(Bitmap catImage, string catName)
    {
        _catImage = catImage;
        _catName = catName;
    }
}