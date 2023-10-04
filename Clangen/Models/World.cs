using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using Clangen.Models.Events;
using Clangen.Models.CatStuff;
using Clangen.Models.CatGroups;

namespace Clangen.Models;


public enum Season
{
    [Description("Newleaf")]
    Newleaf = 0,
    [Description("Greenleaf")]
    Greenleaf = 1,
    [Description("Leaf-fall")]
    Leaffall = 2,
    [Description("Leaf-bare")]
    Leafbare = 3

}


/// <summary>
/// Holds information on a single save. 
/// </summary>
public class World
{
    
    public Dictionary<string, Cat> AllCats = new();
    
    public List<string> FadedIds = new();
    
    
    public Season season { get; private set; } = Season.Newleaf;
    private readonly Season StartingSeason = Season.Newleaf;
    
    public List<SingleEvent> currentEvents { get; set; } = new();
    public List<SingleEvent> medicineDenEvents { get; set; } = new();
    public List<string> patrolled { get; set; } = new();
    public List<string[]> mediated { get; set; } = new();
    
    
    // Groups
    // Groups by ID
    public Dictionary<string, Group> allGroups { get; set; } = new();
    
    // Variables holding groups for easy reference. 
    // Main clan of living cats
    public Clan currentClan { get; set; }
    
    // Afterlives
    public Afterlife starClan { get; set; }
    public Afterlife darkForest { get; set; }
    public Afterlife unknownRes { get; set; }
    
    // Outsiders
    public Outsiders outsiders { get; set; }

    public List<OtherClan> otherClans { get; set; } = new();

    public World(Clan? currentClan = null)
    {
        currentClan ??= new("New");
        this.currentClan = currentClan;
        allGroups.Add(currentClan.ID, currentClan);
        
        // Create the afterlives
        starClan = new("StarClan");
        allGroups.Add(starClan.ID, starClan);
        darkForest = new("Dark Forest");
        allGroups.Add(darkForest.ID, darkForest);
        unknownRes = new("Unknown Residence");
        allGroups.Add(unknownRes.ID, unknownRes);
        
        // Two Other Clans
        for (int i = 0; i < 2; i++)
        {
            otherClans.Add(new OtherClan($"Other{i}"));
            allGroups.Add(otherClans.Last().ID, otherClans.Last());
        }
    }
    
    public Cat? FetchCat(string catID)
    {
        if (AllCats.ContainsKey(catID))
        {
            return AllCats[catID];
        }

        Cat? faded = LoadFadedCat(catID);
        if (faded != null)
        {
            return faded;
        };

        return null;
    }
    
    public Cat? LoadFadedCat(string catID)
    {
        // Load a faded cat, if they are faded.
        return null;
    }

}