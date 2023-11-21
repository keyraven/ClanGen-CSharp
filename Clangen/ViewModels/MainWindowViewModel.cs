﻿using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.SimpleRouter;
using Clangen.Models;

namespace Clangen.ViewModels;

public partial class MainWindowViewModel : MainViewModelBase
{
    public MainWindowViewModel(Game game, HistoryRouter<ViewModelBase> router) : base(game, router)
    {
        
    }
}