using System.Collections.Generic;
using System.Text.Json.Serialization;
using Clangen.Models.CatStuff;

namespace Clangen.Models.CatGroups;

public class Outsiders : Group
{
    [JsonInclude]
    private string _name = string.Empty;

    public override string GetName()
    {
        return _name;
    }

    [JsonConstructor]
    internal Outsiders() : base(false, new CatDictionary()) { }

    public Outsiders(IReadOnlyFetchableObject<string, Cat> allCats, 
        string name = "Cats Outside the Clan") : base(false, allCats)
    {
        this._name = name;
    }
}