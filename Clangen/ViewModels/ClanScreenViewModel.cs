using System;
using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;
using Clangen.Models;
using CommunityToolkit.Mvvm.Input;

namespace Clangen.ViewModels;

public partial class ClanScreenViewModel : MainViewModelBase
{

    public class DisplayCat
    {
        public Bitmap CatImage { get; }
        public string CatName { get; }
        public string CatId { get; }

        public DisplayCat(Bitmap catImage, string catName, string catId)
        {
            CatImage = catImage;
            CatName = catName;
            CatId = catId;
        }
    }
    
    
    [ObservableProperty] 
    private ObservableCollection<DisplayCat> _catTiles = new();
    
    [RelayCommand]
    public void PressCatButton(string catID)
    {
        var catName = _game.currentWorld?.FetchCat(catID).fullName;
        Console.WriteLine(catName);
        Console.WriteLine("Button Pressed");
    }
    
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
            
            CatTiles.Add(new DisplayCat(cat.sprite.ConvertToAvaloniaBitmap(), cat.fullName, cat.ID));
        }
        
    }

    
}