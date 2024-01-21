using System;
using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;
using Clangen.Models;
using CommunityToolkit.Mvvm.Input;

namespace Clangen.ViewModels;

public partial class ClanScreenViewModel : PageViewModelBase
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
        var cat = _game.currentWorld?.FetchCat(catID);
        Console.WriteLine(cat.fullName);
        Console.WriteLine("Button Pressed");
        
        //Wow this looks awful. But it works. 
        _router.GoTo<CatProfileScreenViewModel>().SetCat(cat);
    }
    
    public ClanScreenViewModel(Game game, HistoryRouter<PageViewModelBase> router) : base(game, router)
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