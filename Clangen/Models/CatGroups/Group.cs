using System.Collections.Generic;
using System.Linq;
using Clangen.Models.CatStuff;

namespace Clangen.Models.CatGroups;

public abstract class Group
{
    public readonly string ID;
    private IReadOnlyDictionary<string, Cat> _allCats { get; }

    protected Group(string id, IReadOnlyDictionary<string, Cat> allCats)
    {
        ID = id;
        _allCats = allCats;
    }

    public IReadOnlyCollection<Cat> GetMembers()
    {
        return _allCats.Values.Where(cat => cat.belongGroup == this).ToList().AsReadOnly();
    }
    
    public abstract string GetName();

    public abstract void UpdateCatStatus(Cat cat, Cat.CatStatus oldStatus);
}