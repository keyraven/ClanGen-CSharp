using System;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;
using Clangen.Models;

namespace Clangen.ViewModels;

public partial class ClanScreenViewModel : ViewModelBase
{
    [ObservableProperty] 
    private Bitmap? _catImage;
    
    public ClanScreenViewModel(Game game, HistoryRouter<ViewModelBase> router) : base(game, router)
    {
        Console.WriteLine(game.test);
    }
}