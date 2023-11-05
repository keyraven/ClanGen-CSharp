using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;
using Avalonia.Media.Imaging;
using SkiaSharp;
using Clangen.Models;

namespace Clangen.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected HistoryRouter<ViewModelBase> _router;
    protected Game _game;

    public ViewModelBase(Game game, HistoryRouter<ViewModelBase> router)
    {
        _router = router;
        _game = game;
    }
    
}
