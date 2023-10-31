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
    
    /// <summary>
    /// Updates any group variables when a cat's status chances. Meant to be called
    /// from the cat status property, in set;. This should never need to called outside of that
    /// context. 
    /// </summary>
    /// <param name="cat"> The cat whose status updated. The status property of the cat should already be chanced. </param>
    /// <param name="oldStatus"> The old status of the cat. </param>
    public override void UpdateCatStatus(Cat cat, Cat.CatStatus oldStatus)
    {
        if (oldStatus == Cat.CatStatus.MedicineCat && medicineCats.Contains(cat.ID))
        {
            medicineCats.Remove(cat.ID);
        }
        
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


