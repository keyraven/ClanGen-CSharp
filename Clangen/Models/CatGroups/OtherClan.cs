using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json.Serialization;
using Clangen.Models.CatStuff;

namespace Clangen.Models.CatGroups;

public class OtherClan : Group
{
    private int _reputation = 0;
    
    [JsonConstructor]
    public OtherClan(string ID, bool dead) : base(ID, dead, new CatDictionary())
    {

    }

    public OtherClan(string ID, IReadOnlyFetchableObject<string, Cat> allCats, string prefix) : base(ID, false, allCats)
    {
        this.prefix = prefix;
    }

    public string prefix { get; set; } = string.Empty;
    public string suffix { get; set; } = "Clan";
    
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
    
    public override string GetName()
    {
        return $"{prefix}{suffix}";
    }

    [JsonIgnore]
    public Cat? leader
    {
        get
        {
            return GetMembers().FirstOrDefault(i => i.status == Cat.CatStatus.Leader);
        }
    }

    [JsonIgnore]
    public Cat? deputy
    {
        get
        {
            return GetMembers().FirstOrDefault(i => i.status == Cat.CatStatus.Deputy);
        }
    }

    [JsonIgnore]
    public List<Cat> medicineCats
    {
        get
        {
            return GetMembers().Where(i => i.status == Cat.CatStatus.MedicineCat).ToList();
        }
    }
}