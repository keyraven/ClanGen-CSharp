using System.Collections.Generic;
using Clangen.Models.CatStuff;

namespace Clangen.Models.CatGroups;

public class Outsiders : Group
{
    private string _name;

    public override string GetName()
    {
        return _name;
    }
    
    public Outsiders(string id, IReadOnlyDictionary<string, Cat> allCats, 
        string name = "Cats Outside the Clan") : base(id, false, allCats)
    {
        this._name = name;
    }
}