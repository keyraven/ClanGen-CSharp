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
    
    private Dictionary<string, Cat> _allCats = new();
    private List<string> _fadedIds = new();

    public WorldSettings worldSettings { get; set; } = new();
    
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

    public World(List<Cat> allCats, Clan? currentClan = null, Afterlife? starClan = null, Afterlife? darkForest = null,
        Afterlife? unknownRes = null, Outsiders? outsiders = null, List<OtherClan>? otherClans = null)
    {
        foreach (Cat kitty in allCats)
        {
            _allCats.Add(kitty.Id, kitty);
        }
        
        currentClan ??= new("New");
        this.currentClan = currentClan;
        allGroups.Add(this.currentClan.ID, this.currentClan);
        
        // Create the afterlives
        starClan ??= new("StarClan");
        this.starClan = starClan;
        allGroups.Add(this.starClan.ID, this.starClan);
        
        darkForest ??= new("Dark Forest");
        this.darkForest = darkForest;
        allGroups.Add(this.darkForest.ID, this.darkForest);
        
        unknownRes ??= new("Unknown Residence");
        this.unknownRes = unknownRes;
        allGroups.Add(this.unknownRes.ID, this.unknownRes);
        
        // Outsiders
        outsiders ??= new();
        this.outsiders = outsiders; 
        allGroups.Add(this.outsiders.ID, this.outsiders);
        
        // Two Other Clans
        otherClans ??= new List<OtherClan>() { new OtherClan("Clan1"), new OtherClan("Clan2") };
        this.otherClans = otherClans;
        
        foreach (OtherClan clan in this.otherClans)
        {
            allGroups.Add(clan.ID, clan);
        }
    }
    
    public World(string clanName)
    {
        this.currentClan = new(clanName);
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

    /// <summary>
    /// Adds a cat to the world, both the AllCats Dictionary and to
    /// a group
    /// </summary>
    /// <param name="addCat"> The cat to add </param>
    /// <param name="belongGroup"> The ID of the group to add the cat too. If left null, will add the currentClan </param>
    public void AddCatToWorld(Cat addCat, string? belongGroup = null)
    {
        _allCats.Add(addCat.Id, addCat);
        belongGroup ??= currentClan.ID;
        
        if (allGroups.ContainsKey(belongGroup))
        {
            allGroups[belongGroup].AddMember(addCat);
        }
        else
        {
            currentClan.AddMember(addCat);
        }
    }
    
    /// <summary>
    /// Fetches a cat object based on the cat ID.
    /// Will load faded cats if needed.
    /// </summary>
    /// <param name="catID"></param>
    /// <returns></returns>
    public Cat? FetchCat(string catID)
    {
        if (_allCats.ContainsKey(catID))
        {
            return _allCats[catID];
        }
        
        return LoadFadedCat(catID);
    }
    
    public Cat? LoadFadedCat(string catID)
    {
        // Load a faded cat, if they are faded.
        return null;
    }

    public bool CatIdTaken(string id)
    {
        return _allCats.ContainsKey(id) || _fadedIds.Contains(id);
    }

    public bool CatIdTaken(int id)
    {
        string stringID = id.ToString();
        return CatIdTaken(stringID);
    }

    public List<Cat> GetAllCats()
    {
        return _allCats.Values.ToList();
    }

}

public class WorldSettings
{
    
}