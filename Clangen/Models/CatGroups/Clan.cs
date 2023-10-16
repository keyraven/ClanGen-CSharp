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

    // PLACE TO EASILY HOLD IDs FOR THE LEADER
    // DEPUTY, MEDICINE CATS, EXT
    public string? leader { get; set; }
    public string? deputy { get; set; }
    public List<string> medicineCats { get; set; } = new();
    
    // For creating a new clan, not loading them. 
    public Clan(IReadOnlyDictionary<string, Cat> allCats, string prefix, string? leader = null, string? deputy = null,
        List<string>? medicineCats = null) : base(allCats)
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


