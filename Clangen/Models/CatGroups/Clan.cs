using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Clangen.Models.CatStuff;

namespace Clangen.Models.CatGroups;

public class Clan : Group
{
    // NAME ATTRIBUTES
    private string prefix { get; set; } = string.Empty;
    private string suffix { get; } = "Clan";
    
    public override string GetName()
    {
        return $"{prefix}{suffix}";
    }


    // PLACE TO EASILY HOLD IDs FOR THE LEADER
    // DEPUTY, MEDICINE CATS, EXT
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

    [JsonConstructor]
    public Clan(string ID, bool dead) : base(ID, dead, new CatDictionary())
    {

    }

    public Clan(string ID, IReadOnlyFetchableObject<string, Cat> allCats, string prefix, string? leader = null, string? deputy = null,
        List<string>? medicineCats = null) : base(ID, false, allCats)
    {
        this.prefix = prefix;
    }
    
}


