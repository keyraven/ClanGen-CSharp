using System.Collections.Generic;
using System.Linq;
using Clangen.Models.CatStuff;

namespace Clangen.Models.CatGroups;

public abstract class Group
{
    public readonly string ID;
    
    public readonly bool dead;
    
    private IReadOnlyDictionary<string, Cat> _allCats { get; }

    protected Group(string id, bool dead, IReadOnlyDictionary<string, Cat> allCats)
    {
        ID = id;
        this.dead = dead;
        _allCats = allCats;
    }

    public IReadOnlyCollection<Cat> GetMembers()
    {
        return _allCats.Values.Where(cat => cat.belongGroup == this).ToList().AsReadOnly();
    }
    
    public abstract string GetName();
    
}