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

    public override void UpdateCatStatus(Cat cat, Cat.CatStatus oldStatus)
    {
        throw new System.NotImplementedException();
    }
    
    public Outsiders(string id, IReadOnlyDictionary<string, Cat> allCats, 
        string name = "Cats Outside the Clan") : base(id, allCats)
    {
        this._name = name;
    }
}