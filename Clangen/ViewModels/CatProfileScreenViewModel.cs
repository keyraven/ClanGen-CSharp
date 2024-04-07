using Clangen.Models;
using Clangen.Models.CatStuff;

namespace Clangen.ViewModels;

public partial class CatProfileScreenViewModel : PageViewModelBase
{
    // Would prefer this not to be null. Set dummy cat? 
    public Cat? CurrentCat;
    
    private string _catName = string.Empty;

    public CatProfileScreenViewModel()
    {
    }

    public void SetCat(Cat setCat)
    {
        CurrentCat = setCat;
        //CatName = setCat.fullName;
    }
}