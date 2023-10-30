using System;
using System.Collections.Generic;
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
    
    public override void UpdateCatStatus(Cat cat, Cat.CatStatus oldStatus)
    {
        if (cat.status == Cat.CatStatus.Leader)
        {
            leader = cat.ID;
        }

        if (cat.status == Cat.CatStatus.Deputy)
        {
            deputy = cat.ID;
        }

        if (cat.status == Cat.CatStatus.MedicineCat)
        {
            medicineCats.Add(cat.ID);
        }
    }

    // PLACE TO EASILY HOLD IDs FOR THE LEADER
    // DEPUTY, MEDICINE CATS, EXT
    public string? leader { get; set; }
    public string? deputy { get; set; }
    public List<string> medicineCats { get; set; } = new();
    
    // For creating a new clan, not loading them. 
    public Clan(string id, IReadOnlyDictionary<string, Cat> allCats, string prefix, string? leader = null, string? deputy = null,
        List<string>? medicineCats = null) : base(id, allCats)
    {
        this.prefix = prefix;
        if (medicineCats != null)
        {
            this.medicineCats = medicineCats;
        }

        this.leader = leader;
        this.deputy = deputy;
    }
    
}


