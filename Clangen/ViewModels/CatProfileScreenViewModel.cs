using System;
using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;
using Clangen.Models;
using Clangen.Models.CatStuff;
using CommunityToolkit.Mvvm.Input;

namespace Clangen.ViewModels;

public partial class CatProfileScreenViewModel : PageViewModelBase
{
    public Cat currentCat;

    [ObservableProperty] 
    private string _catName = string.Empty;

    public CatProfileScreenViewModel(Game game, HistoryRouter<PageViewModelBase> router) : base(game, router)
    {
    }

    public void SetCat(Cat setCat)
    {
        currentCat = setCat;
        CatName = setCat.fullName;
    }
}