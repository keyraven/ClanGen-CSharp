using System;
using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using Prism.Regions;
using Clangen.Models;

namespace Clangen.ViewModels;

public partial class ClanScreenViewModel : ViewModelBase, INavigationAware
{

    private IRegionManager _regionManager;
    private Game _game;
    private string test = "old_set";
    
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

    public void OnNavigatedTo(NavigationContext parameters)
    {
        var st = parameters.Parameters.GetValue<string>("test");
        test = st;
    }

    public void OnNavigatedFrom(NavigationContext parameters)
    {
        
    }

    public bool IsNavigationTarget(NavigationContext parameters)
    {
        return true;
    }
    
    //[ObservableProperty] 
    private ObservableCollection<DisplayCat> _catTiles = new();
    
    //[RelayCommand]
    public void PressCatButton(string catID)
    {
        var cat = _game.currentWorld?.FetchCat(catID);
        Console.WriteLine(cat.fullName);
        Console.WriteLine("Button Pressed");
        
        //Wow this looks awful. But it works. 
        //_router.GoTo<CatProfileScreenViewModel>().SetCat(cat);
    }
    
    public ClanScreenViewModel(Game game, IRegionManager regionManager)
    {
        _game = game;
        _regionManager = regionManager;
        
    }

    private void GenerateCatTiles()
    {
        if (_game.currentWorld == null)
        {
            return;
        }
        
        /*
        foreach (var cat in _game.currentWorld.GetAllCats())
        {
            CatTiles.Add(new DisplayCat(cat.sprite.ConvertToAvaloniaBitmap(), cat.fullName, cat.ID));
        }
        */
        
    }

    
}