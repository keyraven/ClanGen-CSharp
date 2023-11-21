using System;
using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;
using Clangen.Models;

namespace Clangen.ViewModels;

public partial class ClanScreenViewModel : MainViewModelBase
{

    [ObservableProperty] 
    private ObservableCollection<CatTileViewModel> _catTiles = new();
    
    public ClanScreenViewModel(Game game, HistoryRouter<ViewModelBase> router) : base(game, router)
    {
        GenerateCatTiles();
    }

    private void GenerateCatTiles()
    {
        if (_game.currentWorld == null)
        {
            return;
        }
        
        foreach (var cat in _game.currentWorld.GetAllCats())
        {
            CatTiles.Add(new CatTileViewModel(cat.sprite.ConvertToAvaloniaBitmap(), cat.fullName));
        }
    }
    
}