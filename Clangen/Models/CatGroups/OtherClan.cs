using System.Collections.Generic;

namespace Clangen.Models.CatGroups;

public class OtherClan : Group
{
    public OtherClan(string prefix, SortedSet<string>? cats = null, string? leader = null, string? deputy = null,
        List<string>? medicineCats = null)
    {
        this.prefix = prefix;
        if (cats != null)
        {
            this.members = cats;
        }

        this.leader = leader;
        this.deputy = deputy;
        if (medicineCats != null)
        {
            this.medicineCats = medicineCats;
        }
    }

    public string prefix { get; set; }
    public string suffix { get; set; } = "Clan";

    public override string name
    {
        get
        {
            return $"{prefix}{suffix}";
        }
    } 

    public string? leader { get; set; }
    public string? deputy { get; set; }
    public List<string> medicineCats { get; set; } = new();
}