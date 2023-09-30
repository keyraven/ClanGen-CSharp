using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Clangen.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _contentViewModel;

    private Dictionary<string, ViewModelBase> Pages = new();

    public MainWindowViewModel()
    {
        this.Pages = new()
        {
            ["StartScreen"] = new StartScreenViewModel(),
            ["ClanScreen"] = new ClanScreenViewModel()
        };
    }


}