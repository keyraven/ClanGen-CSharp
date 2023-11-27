using System.Collections.Generic;
using Clangen.Models.CatStuff;

namespace Clangen.Models.CatGroups;

public class Afterlife : Group
{
    private string _name; 
    
    public override string GetName()
    {
        return _name;
    }

    public Afterlife(IReadOnlyFetchableObject<string, Cat> allCats,  string name = "Afterlife", 
        SortedSet<string>? cats = null) : base(true, allCats)
    {
        this._name = name;
    }
}