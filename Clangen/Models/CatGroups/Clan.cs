using System.Collections.Generic;

namespace Clangen.Models.CatGroups;

public class Clan : Group
{
    // NAME ATTRIBUTES
    public string prefix { get; set; }
    public string suffix { get; set; } = "Clan";
    public override string name
    {
        get { return $"{prefix}{suffix}"; }
    }

    // PLACE TO EASILY HOLD IDs FOR THE LEADER
    // DEPUTY, MEDICINE CATS, EXT
    public string? leader { get; set; }
    public string? deputy { get; set; }
    public List<string> medicineCats { get; set; } = new();
    
    // For creating a new clan, not loading them. 
    public Clan(string prefix, SortedSet<string>? cats = null, string? leader = null, string? deputy = null,
        List<string>? medicineCats = null)
    {
        this.prefix = prefix;
        if (medicineCats != null)
        {
            this.medicineCats = medicineCats;
        }

        this.leader = leader;
        this.deputy = deputy;

        cats ??= new();
        members = cats;

    }
    
}


