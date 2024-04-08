using Clangen.Models;

namespace Clangen.ViewModels;

public class MainViewModel : ViewModelBase
{
    //[ObservableProperty]
    private ViewModelBase _content = default!;

    public MainViewModel()
    {
    }
}
