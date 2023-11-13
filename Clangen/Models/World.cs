using System;
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
public partial class World
{
    
    // GENERATE NEW IDs
    private int _lastCatId = 0;
    private int _lastGroupId = 0;
    
    public string GetNextCatId()
    {
        _lastCatId++;
        return _lastCatId.ToString();
    }

    public string GetNextGroupId()
    {
        _lastGroupId++;
        return _lastGroupId.ToString();
    }
    
    private Dictionary<string, Cat> _allCats = new();
    private List<string> _fadedIds = new();

    public WorldSettings worldSettings { get; set; } = new();
    
    public Season season { get; private set; } = Season.Newleaf;
    private readonly Season StartingSeason = Season.Newleaf;
    public int timeskips { get; set; }
    public int moons { get; }
    
    public List<SingleEvent> currentEvents { get; set; } = new();
    public List<SingleEvent> medicineDenEvents { get; set; } = new();
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

    public World(List<Cat> allCats, int lastCatId = 0, int lastGroupId = 0, Clan? currentClan = null, 
        Afterlife? starClan = null, Afterlife? darkForest = null, Afterlife? unknownRes = null, 
        Outsiders? outsiders = null, List<OtherClan>? otherClans = null)
    {

        _lastCatId = lastCatId;
        _lastGroupId = lastGroupId;
        
        foreach (Cat kitty in allCats)
        {
            _allCats.Add(kitty.ID, kitty);
        }
        
        currentClan ??= new(GetNextGroupId(), _allCats, "New");
        this.currentClan = currentClan;
        allGroups.Add(this.currentClan.ID, this.currentClan);
        
        // Create the afterlives
        starClan ??= new(GetNextGroupId(), _allCats, "StarClan");
        this.starClan = starClan;
        allGroups.Add(this.starClan.ID, this.starClan);
        
        darkForest ??= new(GetNextGroupId(), _allCats, "Dark Forest");
        this.darkForest = darkForest;
        allGroups.Add(this.darkForest.ID, this.darkForest);
        
        unknownRes ??= new(GetNextGroupId(), _allCats,"Unknown Residence");
        this.unknownRes = unknownRes;
        allGroups.Add(this.unknownRes.ID, this.unknownRes);
        
        // Outsiders
        outsiders ??= new("4", _allCats);
        this.outsiders = outsiders; 
        allGroups.Add(this.outsiders.ID, this.outsiders);
        
        // Two Other Clans
        otherClans ??= new List<OtherClan>() { new OtherClan(GetNextGroupId(), _allCats,"Clan1"), new OtherClan(GetNextGroupId(), _allCats,"Clan2") };
        this.otherClans = otherClans;
        
        foreach (OtherClan clan in this.otherClans)
        {
            allGroups.Add(clan.ID, clan);
        }
    }
    
    public World(string clanName, int lastCatId = 0, int lastGroupId = 0)
    {
        _lastCatId = lastCatId;
        _lastGroupId = lastGroupId;
        
        this.currentClan = new(GetNextGroupId(), _allCats, clanName);
        allGroups.Add(currentClan.ID, currentClan);
        
        // Create the afterlives
        starClan = new(GetNextGroupId(), _allCats,"StarClan");
        allGroups.Add(starClan.ID, starClan);
        darkForest = new(GetNextGroupId(), _allCats,"Dark Forest");
        allGroups.Add(darkForest.ID, darkForest);
        unknownRes = new(GetNextGroupId(), _allCats,"Unknown Residence");
        allGroups.Add(unknownRes.ID, unknownRes);
        
        // Two Other Clans
        for (int i = 0; i < 2; i++)
        {
            otherClans.Add(new OtherClan(GetNextGroupId(), _allCats,$"Other{i}"));
            allGroups.Add(otherClans.Last().ID, otherClans.Last());
        }
    }

    /// <summary>
    /// Generates a random cat, and adds them to the world and current clan. 
    /// </summary>
    /// <returns> Created Cat Object. </returns>
    public Cat GenerateRandomCat(Cat.CatStatus status)
    {
        Cat.CatSex sex = Utilities.InverseChanceRoll(2) ? Cat.CatSex.Female : Cat.CatSex.Male;

        int timeskips = 0;
        switch (status)
        {
            case Cat.CatStatus.Kit:
                timeskips = Utilities.RandomInt(Cat.AgeTimeskips[Cat.CatAge.Newborn][0], 
                    Cat.AgeTimeskips[Cat.CatAge.Kitten][1]);
                break;
            case Cat.CatStatus.Apprentice:
                // Fallthrough
            case Cat.CatStatus.MedicineCatApprentice:
                // Fallthrough
            case Cat.CatStatus.MediatorApprentice:
                timeskips = Utilities.RandomInt(Cat.AgeTimeskips[Cat.CatAge.Adolescent]);
                break;
            case Cat.CatStatus.Elder:
                timeskips = Utilities.RandomInt(Cat.AgeTimeskips[Cat.CatAge.Senior]);
                break;
            default:
                timeskips = Utilities.RandomInt(Cat.AgeTimeskips[Cat.CatAge.YoungAdult][0], 
                    Cat.AgeTimeskips[Cat.CatAge.SeniorAdult][1]);
                break;
            
        }
        
        Cat newCat = new(GetNextCatId(), belongWorld: this, belongGroup: currentClan, timeskips: timeskips, sex: sex,
            status: status);
        return newCat;
    }
    
    public void PopulateClan()
    {
        // Generate a clan for testing purposes. 
        GenerateRandomCat(Cat.CatStatus.Leader);
        GenerateRandomCat(Cat.CatStatus.Deputy);
        GenerateRandomCat(Cat.CatStatus.MedicineCat);

        for (int i = 0; i < 10; i++)
        {
            GenerateRandomCat(Cat.CatStatus.Warrior);
        }

        GenerateRandomCat(Cat.CatStatus.Apprentice);
        GenerateRandomCat(Cat.CatStatus.Apprentice);
        GenerateRandomCat(Cat.CatStatus.Kit);
        GenerateRandomCat(Cat.CatStatus.Kit);
    }

    /// <summary>
    /// Kills a cat, sorting them into an afterlife. 
    /// </summary>
    public void KillCat()
    {
        
    }
    
    
    /// <summary>
    /// Adds a cat to the collection of all cats in the world
    /// Mostly called in the cat constructor. 
    /// </summary>
    /// <param name="addCat"> The cat to add </param>
    public void AddCatToWorld(Cat addCat)
    {
        _allCats.Add(addCat.ID, addCat);
    }
    
    /// <summary>
    /// Remove all the cats from the world. 
    /// </summary>
    public void ClearWorld()
    {
        _allCats.Clear();
    }
    
    /// <summary>
    /// Fetches a cat object based on the cat ID.
    /// Will load faded cats if needed.
    /// </summary>
    /// <param name="catID"></param>
    /// <returns></returns>
    public Cat FetchCat(string catId)
    {
        if (_allCats.ContainsKey(catId))
        {
            return _allCats[catId];
        }
        
        return LoadFadedCat(catId);
    }
    
    public Cat LoadFadedCat(string catID)
    {
        // Load a faded cat, if they are faded.
        throw new NotImplementedException("Faded Cat Loading not Implemented. ");
    }
    
    public IReadOnlyCollection<Cat> GetAllCats()
    {
        return _allCats.Values.ToList().AsReadOnly();
    }

    public IReadOnlyCollection<string> GetAllCatIds()
    {
        return _allCats.Keys.ToList().AsReadOnly();
    }

}

public class WorldSettings
{
    
}

