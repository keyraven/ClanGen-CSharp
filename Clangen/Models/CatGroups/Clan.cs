using System;
using System.Collections.Generic;
using System.Linq;
using Clangen.Models.CatStuff;

namespace Clangen.Models.CatGroups;

public class Clan : Group
{
    // NAME ATTRIBUTES
    private string prefix { get; set; }
    private string suffix { get; set; } = "Clan";
    
    public override string GetName()
    {
        return $"{prefix}{suffix}";
    }
    
    
    // PLACE TO EASILY HOLD IDs FOR THE LEADER
    // DEPUTY, MEDICINE CATS, EXT
    public Cat? leader
    {
        get
        {
            return GetMembers().FirstOrDefault(i => i.status == Cat.CatStatus.Leader);
        }
    }

    public Cat? deputy
    {
        get
        {
            return GetMembers().FirstOrDefault(i => i.status == Cat.CatStatus.Deputy);
        }
    }
    public List<Cat> medicineCats
    {
        get
        {
            return GetMembers().Where(i => i.status == Cat.CatStatus.MedicineCat).ToList();
        }
    }
    
    // For creating a new clan, not loading them. 
    public Clan(string id, IReadOnlyDictionary<string, Cat> allCats, string prefix, string? leader = null, string? deputy = null,
        List<string>? medicineCats = null) : base(id, allCats)
    {
        this.prefix = prefix;
    }
    
}


