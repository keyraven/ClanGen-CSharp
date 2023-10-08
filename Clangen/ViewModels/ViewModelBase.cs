using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;
using Avalonia.Media.Imaging;
using SkiaSharp;

namespace Clangen.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected HistoryRouter<ViewModelBase> _router;

    public ViewModelBase(HistoryRouter<ViewModelBase> router)
    {
        _router = router;
    }
    
}
