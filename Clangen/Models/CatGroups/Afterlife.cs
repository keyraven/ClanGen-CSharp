using System.Collections.Generic;
using System.Text.Json.Serialization;
using Clangen.Models.CatStuff;

namespace Clangen.Models.CatGroups;

public class Afterlife : Group
{
    [JsonInclude]
    private string _name = string.Empty;
    
    public override string GetName()
    {
        return _name;
    }

    [JsonConstructor]
    internal Afterlife() : base(true, new CatDictionary()) { }
    public Afterlife(IReadOnlyFetchableObject<string, Cat> allCats,  string name = "Afterlife", 
        SortedSet<string>? cats = null) : base(true, allCats)
    {
        this._name = name;
    }
}