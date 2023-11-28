using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Clangen.Models.CatStuff;

public class Name
{
    private class NameDetails
    {
        public List<string> normalPrefixes { get; set; } = new();
        public List<string> normalSuffixes { get; set; } = new();
        public Dictionary<Cat.CatStatus, string> specialSuffixes { get; set; } = new();
        public List<string> inappropriateNames { get; set; } = new();

        public List<string> lonerNames { get; set; } = new();

    }
    
    // Not sure how to remove the "possible null reference" warning. 
    private static readonly NameDetails details = JsonSerializer.Deserialize<NameDetails>(File.ReadAllText("Resources/names.json"));

    public string prefix { get; set; } = string.Empty;
    public string suffix { get; set; } = string.Empty;
    public bool specSuffixHidden { get; set; } = false;
    public Cat? OwnerCat { get; set; }

    /// <summary>
    /// Returns the full name of the cat, with prefix, suffix, and any special
    /// suffixes attached. 
    /// </summary>
    [JsonIgnore]
    public string fullName
    {
        get
        {
            if (OwnerCat is null)
            {
                return $"{prefix}{suffix}";
            }
            if (!specSuffixHidden && details.specialSuffixes.ContainsKey(OwnerCat.status))
            {
                return $"{prefix}{details.specialSuffixes[OwnerCat.status]}";
            }

            return $"{prefix}{suffix}";
        }
    }

    [JsonConstructor]
    internal Name() { }

    /// <summary>
    /// Holds information on the name of a cat. Constructor, on it's own, does not check for name
    /// validity. 
    /// </summary>
    /// <param name="cat"></param>
    /// <param name="prefix"></param>
    /// <param name="suffix"></param>
    /// <param name="specSuffixHidden"></param>
    /// <param name="nameStatus">  </param>
    public Name(string? prefix, string? suffix, bool specSuffixHidden = false,
        Cat? cat = null)
    {
        prefix ??= "";
        suffix ??= "";
        this.prefix = prefix;
        this.suffix = suffix;
        this.specSuffixHidden = specSuffixHidden;
        this.OwnerCat = cat;
    }

    /// <summary>
    /// Generate a random new name. Checks for valid names. 
    /// </summary>
    /// <param name="cat"> Reference to cat object. Can be null </param>
    /// <param name="loner"> If true, will generate the loner-type name, where the suffix is blank and the prefix is chosen from the loner names list. </param>
    /// <returns></returns>
    public static Name GenerateRandomName(Cat? cat, bool loner = false)
    {
        var chosenPrefix = loner ? Utilities.ChoseRandom(details.lonerNames) : Utilities.ChoseRandom(details.normalPrefixes);
        var chosenSuffix = loner ? "" : Utilities.ChoseRandom(details.normalSuffixes);


        // Check number of loops to prevent falling into a loop here
        // If you can't get a good name after 30 tries, something else is wrong. 
        int i = 0;
        while (!Name.IsNameValid(chosenPrefix, chosenSuffix) | i > 30)
        {
            chosenPrefix = Utilities.ChoseRandom(details.normalPrefixes);
            i++;
        }
        
        return new Name(chosenPrefix, chosenSuffix, false, cat);
    }
    
    /// <summary>
    /// Determines if a certain set of prefix and suffixes are valid together. 
    /// </summary>
    /// <param name="prefix"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    private static bool IsNameValid(string prefix, string suffix)
    {
        string lowerPrefix = prefix.ToLower();
        string lowerSuffix = suffix.ToLower();
        
        // Totally empty name are not valid
        if (prefix == "" & suffix == "")
        {
            return false;
        }

        // Check for names where the prefix and suffix are the same.
        if (lowerPrefix == lowerSuffix)
        {
            return false;
        }
    
        //Check names where the suffix is a substring of the prefix, or vis versa
        if (lowerPrefix.Contains(lowerSuffix) | lowerSuffix.Contains(lowerPrefix))
        {
            return false;
        }

        // Check if the same is inappropriate. 
        if (details.inappropriateNames.Contains($"{lowerPrefix}{lowerSuffix}"))
        {
            return false;
        }

        //Check if the name results in three of the same letter in a row. 
        
        // -- First potential three-letter - two letters from prefix, one from suffix
        var half1 = prefix.Length >= 2 ? prefix[^2..] : "";
        var half2 = suffix.Length >= 1 ? suffix[0].ToString() : "";
        var possible = $"{half1}{half2}";

        if (possible != "" && possible.All(ch => ch == possible[0]))
        {
            return false;
        }
        
        // -- Second potential three-letter - one letters from prefix, two from suffix
        half1 = prefix.Length >= 1? prefix[0].ToString() : "";
        half2 = suffix.Length >= 2 ? suffix[^2..] : "";
        possible = $"{half1}{half2}";
        
        if (possible != "" && possible.All(ch => ch == possible[0]))
        {
            return false;
        }
        
        return true;
    }

}


