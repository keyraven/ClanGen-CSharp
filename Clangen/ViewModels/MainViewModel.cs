using Clangen.Models;

namespace Clangen.ViewModels;

public class MainViewModel : PageViewModelBase
{
    //[ObservableProperty]
    private PageViewModelBase _content = default!;

    public MainViewModel()
    {
    }
}
