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
    private string suffix { get; set; } = "Clan";
    
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
    internal Clan() : base(false, new CatDictionary()) { }

    // For creating a new clan, not loading them. 
    public Clan(IReadOnlyFetchableObject<string, Cat> allCats, string prefix, string? leader = null, string? deputy = null,
        List<string>? medicineCats = null) : base(false, allCats)
    {
        this.prefix = prefix;
    }
    
}


