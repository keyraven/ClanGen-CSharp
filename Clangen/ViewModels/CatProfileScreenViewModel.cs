using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;
using Clangen.Models;
using Clangen.Models.CatStuff;

namespace Clangen.ViewModels;

public partial class CatProfileScreenViewModel : PageViewModelBase
{
    // Would prefer this not to be null. Set dummy cat? 
    public Cat? CurrentCat;

    [ObservableProperty] 
    private string _catName = string.Empty;

    public CatProfileScreenViewModel(Game game, HistoryRouter<PageViewModelBase> router) : base(game, router)
    {
    }

    public void SetCat(Cat setCat)
    {
        CurrentCat = setCat;
        CatName = setCat.fullName;
    }
}