using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Clangen.Models.CatStuff;

enum Trait
{
    
}

public class Personality
{
    private class TraitInfo (Dictionary<Trait, List<int>> normalTraits, Dictionary<Trait, List<int>> kittenTraits)
    {
        public Dictionary<Trait, List<int>> normalTraits { get; init; } = normalTraits;
        public Dictionary<Trait, List<int>> kittenTraits { get; init; } = kittenTraits;
    }

    //private static readonly TraitInfo? TraitRanges = JsonSerializer.Deserialize<TraitInfo>(File.ReadAllText("file_name.json"));

    private static readonly int[] facetRange = new int[] { 0, 16 };

    private int _aggression = 0;
    private int _neuroticism = 0;
    private int _sociability = 0;
    private bool kit = false;

    public string? trait;
    public int aggression
    {
        get { return _aggression; }
        set
        {
            _aggression = AdjustValueToRange(value);
        }
    }
    public int neuroticism
    {
        get { return _neuroticism; }
        set
        {
            _neuroticism = AdjustValueToRange(value);
        }
    }
    
    public int sociability
    {
        get { return _sociability; }
        set
        {
            _sociability = AdjustValueToRange(value);
        }
    }

    private int AdjustValueToRange(int value)
    {
        if (value < facetRange[0])
        {
            return facetRange[0];
        }

        if (value > facetRange[1])
        {
            return facetRange[1];
        }

        return value;
    }

    public Personality(string trait)
    {
        this.trait = trait;
    }

    private bool IsTraitValid()
    {
        return true;
    }

    private void SetTrait()
    {

    }


    public void SetAge(bool kit)
    {
        this.kit = kit;
    }
}
