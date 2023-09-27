using System.Collections.Generic;
using System.ComponentModel;
using Clangen.Models.CatStuff;
using Clangen.Models.Events; 

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

public class Clan
{
    // NAME ATTRIBUTES
    public readonly string Prefix = "";
    public readonly string Suffix = "Clan";
    public string clanName
    {
        get { return $"{Prefix}{Suffix}"; }
    }

    // PLACE TO EASILY HOLD IDs FOR THE LEADER
    // DEPUTY, MEDICINE CATS, EXT
    public string? leader { get; set; }
    public string? deputy { get; set; }
    public List<string>? medicineCats { get; set; }

    private int _age = 0;
    public int age
    {
        get { return _age; }
        set
        {
            _age = value;
            // For future reference, and for beginners
            season = (Season)((age % 12) / 3);
        }
    }

    public Season season { get; private set; } = Season.Newleaf;
    private Season StartingSeason = Season.Newleaf;

    // Switch to list of SingleEvents
    public List<SingleEvent> currentEvents { get; set; } = new();
    public List<SingleEvent> medicineDenEvents { get; set; } = new();
    public List<string> patrolled { get; set; } = new();
    public List<string[]> mediated { get; set; } = new();

    public Dictionary<string, List<string>> griefStrings { get; set; } = new();

    public List<string> deadCats { get; set; } = new();

    // Dictionary to access cats quickly, and a list to keep the order. 
    public Dictionary<string, Cat> AllCats = new();
    public List<Cat> AllCatsList = new();

    public List<string> FadedIds = new();

    // For creating a new clan, not loading them. 
    public Clan(string prefix, List<Cat>? cats = null, string? suffix = null, string? leader = null, string? deputy = null,
        List<string>? medicineCats = null)
    {
        this.Prefix = prefix;
        if (suffix != null)
        {
            this.Suffix = suffix;
        }
        if (medicineCats != null)
        {
            this.medicineCats = medicineCats;
        }

        this.leader = leader;
        this.deputy = deputy;
        this.medicineCats = medicineCats;

        AllCatsList = cats is null ? new List<Cat>() : cats;
        foreach (Cat c in AllCatsList)
        {
            AllCats.Add(c.Id, c);
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

    public void SaveClan()
    {
        //Save a Clan
    }

    public Cat? LoadFadedCat(string catID)
    {
        // Load a faded cat, if they are faded.
        return null;
    }

}


