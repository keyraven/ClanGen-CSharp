using System.Collections.Generic;
using System.Text.Json.Serialization;
using Clangen.Models.CatStuff;

namespace Clangen.Models.CatGroups;

public class Afterlife : Group
{
    [JsonInclude]
    [JsonPropertyName("name")]
    private string _name = string.Empty;

    private int _reputation = 0;
    
    public override string GetName()
    {
        return _name;
    }

    public int reputation
    {
        get { return _reputation; }
        set
        {
            _reputation = value;
            if (_reputation > 50)
            {
                _reputation = 50;
            }
            else if (_reputation < -50)
            {
                _reputation = -50;
            }
            
        }
    }

    [JsonConstructor]
    public Afterlife(string ID, bool dead) : base(ID, dead, new CatDictionary())
    {

    }

    public Afterlife(string ID, IReadOnlyFetchableObject<string, Cat> allCats,  string name = "Afterlife", 
        SortedSet<string>? cats = null) : base(ID, true, allCats)
    {
        this._name = name;
    }
}