using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Clangen.Models.CatStuff;

public class Name
{
    private class NameDetails
    {
        public List<string> normalPrefixes { get; set; } = new();
        public List<string> normalSuffixes { get; set; } = new();
        public Dictionary<Cat.CatStatus, string>? specialSuffixes { get; set; } = new();
        public List<string>? inappropriateNames { get; set; } = new();
    }
    
    // Not sure how to remove the "possible null reference" warning. 
    private static readonly NameDetails details = JsonSerializer.Deserialize<NameDetails>(
        File.ReadAllText("Resources/names.json"));

    public string Prefix;
    public string Suffix;
    public bool SpecSuffixHidden;
    public Cat? OwnerCat;

    /// <summary>
    /// Returns the full name of the cat, with prefix, suffix, and any special
    /// suffixes attached. 
    /// </summary>
    public string fullName
    {
        get
        {
            if (OwnerCat is null)
            {
                return $"{Prefix}{Suffix}";
            }
            if (!SpecSuffixHidden && details.specialSuffixes.ContainsKey(OwnerCat.status))
            {
                return $"{Prefix}{details.specialSuffixes[OwnerCat.status]}";
            }

            return $"{Prefix}{Suffix}";
        }
    }

    // CONSTRUCTOR
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
        this.Prefix = prefix;
        this.Suffix = suffix;
        this.SpecSuffixHidden = specSuffixHidden;
        this.OwnerCat = cat;
    }

    /// <summary>
    /// Generate a random new name. Checks for valid names. 
    /// </summary>
    /// <param name="nameStatus"></param>
    /// <returns></returns>
    public static Name GenerateRandomName(Cat? cat)
    {
        string chosenPrefix = Utilities.ChoseRandom(details.normalPrefixes);
        string chosenSuffix = Utilities.ChoseRandom(details.normalSuffixes);


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

        if (lowerPrefix == lowerSuffix)
        {
            // Double word names are not valid
            return false;
        }

        if (lowerPrefix.Contains(lowerSuffix) | lowerSuffix.Contains(lowerPrefix))
        {
            // Names where the prefix is a substring of the suffix, or vis versa, are 
            // not valid names. 
            return false;
        }

        if (details.inappropriateNames.Contains($"{lowerPrefix}{lowerSuffix}"))
        {
            // Name is in the inappropriate names list. 
            return false;
        }

        // TODO - Check for triple letters. 
        // TODO - Check for double-animal. 

        return true;
    }

}


