using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Clangen.Models.CatStuff;

namespace Clangen.Models.CatGroups;

public abstract class Group
{
    public readonly bool dead;
    public readonly string ID;

    private IReadOnlyFetchableObject<string, Cat> _allCats;

    protected Group(string ID, bool dead, IReadOnlyFetchableObject<string, Cat> allCats)
    {
        this.ID = ID;
        this.dead = dead;
        _allCats = allCats;
    }

    public void SetAllCats(IReadOnlyFetchableObject<string, Cat> allCats)
    {
        _allCats = allCats; 
    }

    public IReadOnlyCollection<Cat> GetMembers()
    {
        return _allCats.Values.Where(cat => cat.belongGroup == this).ToList().AsReadOnly();
    }

    public Cat FetchCat(string catId) => _allCats.FetchCat(catId);
    
    public abstract string GetName();
    
}