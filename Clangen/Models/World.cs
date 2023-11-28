using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Clangen.Models.Events;
using Clangen.Models.CatStuff;
using Clangen.Models.CatGroups;
using Microsoft.CodeAnalysis.Scripting.Hosting;

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
    
    private int _lastCatId = 0;
    //private int _lastGroupId = 0;
    private readonly Season _startingSeason = Season.Newleaf;
    private int _timeskips = 0;
    
    public string GetNextCatId()
    {
        _lastCatId++;
        return _lastCatId.ToString();
    }

    /*
    public string GetNextGroupId()
    {
        _lastGroupId++;
        return _lastGroupId.ToString();
    }
    */

    [JsonInclude]
    private CatDictionary _allCats = new();
    
    private List<string> _fadedIds = new();
    
    public WorldSettings worldSettings { get; set; } = new();
    
    public Season season { get; private set; } = Season.Newleaf;
    
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

    public List<SingleEvent> currentEvents { get; set; } = new() { new SingleEvent("Test") };
    public List<SingleEvent> medicineDenEvents { get; set; } = new();
    public List<string[]> mediated { get; set; } = new();
    public readonly GameMode WorldGameMode = GameMode.Expanded;


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

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal World() 
    {
        // for Json purposes ONLY. Not be to be used otherwise.  
        currentClan = new(_allCats, "Clan");
        starClan = new(_allCats);
        darkForest = new(_allCats);
        unknownRes = new(_allCats);
        outsiders = new(_allCats);
    }
    
    public World(List<Cat> allCats, GameMode worldGameMode, int lastCatId = 0,  
        Clan? currentClan = null, Afterlife? starClan = null, Afterlife? darkForest = null, 
        Afterlife? unknownRes = null, Outsiders? outsiders = null, List<OtherClan>? otherClans = null)
    {

        _lastCatId = lastCatId;
        
        foreach (Cat kitty in allCats)
        {
            _allCats.Add(kitty.ID, kitty);
        }

        this.WorldGameMode = worldGameMode;
        
        currentClan ??= new(_allCats, "New");
        this.currentClan = currentClan;
        
        // Create the afterlives
        starClan ??= new(_allCats, "StarClan");
        this.starClan = starClan;
        
        darkForest ??= new( _allCats, "Dark Forest");
        this.darkForest = darkForest;
        
        unknownRes ??= new(_allCats,"Unknown Residence");
        this.unknownRes = unknownRes;
        
        // Outsiders
        outsiders ??= new( _allCats);
        this.outsiders = outsiders; 
        
        // Two Other Clans
        otherClans ??= new List<OtherClan>() { new OtherClan(_allCats,"Clan1"), new OtherClan(_allCats,"Clan2") };
        this.otherClans = otherClans;
        
    }
    
    public World(string clanName, int lastCatId = 0)
    {
        _lastCatId = lastCatId;
        
        this.currentClan = new(_allCats, clanName);
        
        // Create the afterlives
        starClan = new(_allCats,"StarClan");
        darkForest = new( _allCats,"Dark Forest");
        unknownRes = new( _allCats,"Unknown Residence");
        outsiders = new (_allCats);
        
        // Two Other Clans
        for (int i = 0; i < 2; i++)
        {
            otherClans.Add(new OtherClan(_allCats,$"Other{i}"));
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
    public Cat FetchCat(string catId) => _allCats.FetchCat(catId);
    
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

