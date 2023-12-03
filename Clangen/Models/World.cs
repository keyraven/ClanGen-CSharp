using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Clangen.Models.Events;
using Clangen.Models.CatStuff;
using Clangen.Models.CatGroups;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using System.IO;

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

public enum GameMode
{
    [Description("Classic Mode")]
    Classic = 0,
    [Description("Expanded Mode")]
    Expanded = 1,
    [Description("Cruel Season")]
    CruelSeason= 2,
}


/// <summary>
/// Holds information on a single save. 
/// </summary>
public partial class World
{ 
    private const int TimeskipsPerSeason = 6; 
    private readonly Season _startingSeason = Season.Newleaf;
    private int _timeskips = 0;
    private readonly List<OtherClan> _otherClans = new List<OtherClan>();
    private CatDictionary _allCats = new();
    private string? _saveFolderName = null;

    [JsonInclude]
    [JsonPropertyName("lastCatId")]
    private int _lastCatId = 0;

    [JsonInclude]
    [JsonPropertyName("lastGroupId")]
    private int _lastGroupId = 0;

    [JsonInclude]
    [JsonPropertyName("allCats")]
    private IReadOnlyCollection<Cat> _allCatsForSerialize => GetAllCats();

    // PUBLIC

    // If null, the world has not yet been saved. 
    [JsonIgnore]
    public string? saveFolderName { get; set; }

    [JsonIgnore]
    public Season season { get; private set; } = Season.Newleaf;

    public WorldSettings worldSettings { get; set; } = new();

    public int timeskips
    {
        get
        {
            return _timeskips;
        }
        set
        {
            _timeskips = value;
            season = (Season)((_timeskips / TimeskipsPerSeason + (int)_startingSeason) % Enum.GetValues(typeof(Season)).Length);
        }
    }
    
    public float moons
    {
        get { return (float)timeskips / 2; }
    }

    public List<SingleEvent> currentEvents { get; } = new() { new SingleEvent("Test") };
    public List<SingleEvent> medicineDenEvents { get; } = new();
    public List<string[]> mediated { get; } = new();
    public readonly GameMode WorldGameMode = GameMode.Expanded;


    // Variables holding groups for easy reference. 
    // Main clan of living cats
    public Clan currentClan { get; }
    
    // Afterlives
    public Afterlife starClan { get; }
    public Afterlife darkForest { get; }
    public Afterlife unknownRes { get; }
    
    // Outsiders
    public Outsiders outsiders { get; }
    
    public IReadOnlyList<OtherClan> otherClans
    {
        get { return _otherClans.AsReadOnly(); }
    }
    
    public World(List<Cat> allCats, GameMode worldGameMode, int lastCatId = 0,
        int lastGroupId = 0, Clan? currentClan = null, Afterlife? starClan = null, Afterlife? darkForest = null, 
        Afterlife? unknownRes = null, Outsiders? outsiders = null, List<OtherClan>? otherClans = null)
    {

        _lastCatId = lastCatId;
        _lastGroupId = lastGroupId;
        
        foreach (Cat kitty in allCats)
        {
            _allCats.Add(kitty.ID, kitty);
        }

        this.WorldGameMode = worldGameMode;
        
        currentClan ??= new(GetNextGroupId(), _allCats, "New");
        this.currentClan = currentClan;
        
        // Create the afterlives
        starClan ??= new(GetNextGroupId(), _allCats, "StarClan");
        this.starClan = starClan;
        
        darkForest ??= new(GetNextGroupId(), _allCats, "Dark Forest");
        this.darkForest = darkForest;
        
        unknownRes ??= new(GetNextGroupId(), _allCats,"Unknown Residence");
        this.unknownRes = unknownRes;
        
        // Outsiders
        outsiders ??= new(GetNextGroupId(), _allCats);
        this.outsiders = outsiders; 
        
        // Two Other Clans
        otherClans ??= new List<OtherClan>() { new OtherClan(GetNextGroupId(), _allCats,"Clan1"), new OtherClan(GetNextGroupId(), _allCats,"Clan2") };
        this._otherClans = otherClans;
        
    }
    
    public World(string clanName, int lastCatId = 0, int lastGroupId = 0)
    {
        _lastCatId = lastCatId;
        _lastGroupId = lastGroupId;
        
        this.currentClan = new(GetNextGroupId(), _allCats, clanName);
        
        // Create the afterlives
        starClan = new(GetNextGroupId(), _allCats,"StarClan");
        darkForest = new(GetNextGroupId(), _allCats,"Dark Forest");
        unknownRes = new(GetNextGroupId(), _allCats,"Unknown Residence");
        outsiders = new (GetNextGroupId(), _allCats);
        
        // Two Other Clans
        for (int i = 0; i < 2; i++)
        {
            _otherClans.Add(new OtherClan(GetNextGroupId(), _allCats,$"Other{i}"));
        }
    }

    // This is only to be used for JSON deserlization. 
    [JsonConstructor]
    public World(IReadOnlyCollection<Cat> _allCatsForSerialize, Clan currentClan, Afterlife starClan, Afterlife darkForest,
        Afterlife unknownRes, Outsiders outsiders, IReadOnlyList<OtherClan> otherClans)
    {
        foreach (var cat in _allCatsForSerialize)
        {
            _allCats.Add(cat.ID, cat);
        }
        
        currentClan.SetAllCats(_allCats);
        this.currentClan = currentClan;
        starClan.SetAllCats(_allCats);
        this.starClan = starClan;
        darkForest.SetAllCats(_allCats);
        this.darkForest = darkForest;
        unknownRes.SetAllCats(_allCats);
        this.unknownRes = unknownRes;
        outsiders.SetAllCats(_allCats);
        this.outsiders = outsiders;

        foreach (var otherClan in otherClans)
        {
            otherClan.SetAllCats(_allCats);
        }
        _otherClans = otherClans.ToList();

    }

    public WorldSummary GetWorldSummary()
    {
        return new WorldSummary(currentClan.GetName(), GetAllCats().Count, moons);
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
        
        Cat newCat = new(GetNextCatId(), belongGroup: currentClan, timeskips: timeskips, sex: sex,
            status: status, pelt: Pelt.GenerateRandomPelt());
        AddCatToWorld(newCat);
        return newCat;
    }
    
    /// <summary>
    /// TESTING FUNCTION
    /// </summary>
    public void PopulateClan()
    {
        Console.WriteLine("Populating...");
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

        foreach (var value in _allCats)
        {
            Console.WriteLine($"{value.Value.fullName} {value.Value.pelt.peltPattern} {value.Value.pelt.peltColor}");
        }
        Console.WriteLine("Done");
    }
    
    /// <summary>
    /// Adds a cat to the collection of all cats in the world
    /// Called in the cat constructor. 
    /// </summary>
    /// <param name="addCat"> The cat to add </param>
    public void AddCatToWorld(Cat addCat)
    {
        _allCats.Add(addCat.ID, addCat);
    }

    public void RemoveCatFromWorld(string catID)
    {
        _allCats.Remove(catID);
    }

    public void RemoveCatFromWorld(Cat removeCat)
    {
        RemoveCatFromWorld(removeCat.ID);
    }

    public void RemoveAllCatsFromWorld()
    {
        _allCats.Clear();
    }

    public string GetNextCatId() => (++_lastCatId).ToString();

    public string GetNextGroupId() => (++_lastGroupId).ToString();

    /// <summary>
    /// Get a group from it's ID. 
    /// </summary>
    /// <param name="groupID"></param>
    /// <returns></returns>
    public Group FetchGroup(string groupID)
    {
        if (groupID == currentClan.ID) { return currentClan; }
        if (groupID == starClan.ID) { return starClan; }
        if (groupID == unknownRes.ID) { return unknownRes; }
        if (groupID == darkForest.ID) { return darkForest; }
        if (groupID == outsiders.ID) { return outsiders; }

        var outsiderGroup = otherClans.Where(i=>i.ID == groupID).FirstOrDefault();
        if (outsiderGroup != null) { return  outsiderGroup; }

        throw new ArgumentException($"Group with the ID {groupID} was not found");
    }

    /// <summary>
    /// After initial deseralization, cat objects only have a groupID, but not the group object. 
    /// This assigns then the proper group object. 
    /// </summary>
    public void ReplaceDeseralizedGroupIDWithGroupObjects()
    {
        foreach (var catValuePair in _allCats)
        {
            catValuePair.Value.SetGroupBasedOnDeseralizedGroupID(FetchGroup);
        }
    }

    /// <summary>
    /// Set the directory where faded cats are located. 
    /// </summary>
    /// <param name="path"></param>
    public void SetFadedCatPath(string path)
    {
        _allCats.fadedCatPath = path;
    }

    /// <summary>
    /// Fetches a cat object based on the cat ID.
    /// Will load faded cats if needed.
    /// </summary>
    /// <param name="catID"></param>
    /// <returns></returns>
    public Cat FetchCat(string catId) => _allCats.FetchCat(catId);

    public IReadOnlyCollection<Cat> GetAllCats()
    {
        return _allCats.Values.Where(i => !i.faded).ToList().AsReadOnly();
    }

    public IReadOnlyCollection<string> GetAllCatIds()
    {
        return _allCats.Keys.Where(i => !_allCats[i].faded).ToList().AsReadOnly();
    }

    public IReadOnlyCollection<Cat> GetCatsToFade()
    {
        return _allCats.Values.Where(i => i.faded).ToList().AsReadOnly();
    }

}

public class WorldSettings
{
    
}

public struct WorldSummary
{
    public string CurrentClanName { get; init; }
    public int CatNumber { get; init; }
    public float WorldMoons { get; init; }

    [JsonConstructor]
    public WorldSummary(string currentClanName, int catNumber, float worldMoons) => 
        (CurrentClanName, CatNumber, WorldMoons) = (currentClanName, catNumber, worldMoons);
}