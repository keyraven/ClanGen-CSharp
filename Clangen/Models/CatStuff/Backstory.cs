using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Clangen.Models.CatStuff;

public class Backstory
{
    private IReadOnlyDictionary<BackstoryType, string> _defaultShortText = new Dictionary<BackstoryType, string>()
    {
        [BackstoryType.Founder] = "Clan founder",
        [BackstoryType.Clanborn] = "Clanborn",
        [BackstoryType.OtherClan] = "Originally from another clan",
        [BackstoryType.Abandoned] = "Abandoned",
        [BackstoryType.Abandoned & BackstoryType.JoinedAsKit] = "Abandoned",
        [BackstoryType.Refugee] = "Refugee",
        [BackstoryType.Refugee & BackstoryType.OtherClan] = "Refugee",
        [BackstoryType.Refugee & BackstoryType.Loner] = "Refugee",
        [BackstoryType.Refugee & BackstoryType.Kittypet] = "Refugee",
        [BackstoryType.Refugee & BackstoryType.Rogue] = "Refugee",
        [BackstoryType.Disgraced] = "Disgraced",
    };
    
    private IReadOnlyDictionary<BackstoryType, string> _defaultLongText = new Dictionary<BackstoryType, string>()
    {
        [BackstoryType.Founder] = "m_c is one of the founding members of the Clan.",
        [BackstoryType.Clanborn] = "m_c was born into the Clan.",
        [BackstoryType.OtherClan] = "m_c was originally from another Clan",
        [BackstoryType.Abandoned] = "m_c was abandoned, and the c_n took them in. ",
        [BackstoryType.Abandoned & BackstoryType.JoinedAsKit] = "m_c was abandoned as a kit, and c_n took them in.",
        [BackstoryType.Refugee] = "m_c joined the clan after to escape a disaster or tragedy.",
        [BackstoryType.Refugee & BackstoryType.OtherClan] = "m_c used to live in another Clan, but fled when tragedy struck their previous Clan.",
        [BackstoryType.Refugee & BackstoryType.Loner] = "m_c used to be a loner and joined the Clan to escape a disaster.",
        [BackstoryType.Refugee & BackstoryType.Kittypet] = "m_c used to be a kittypet and joined the Clan to escape a disaster.",
        [BackstoryType.Refugee & BackstoryType.Rogue] = "m_c used to be a rogue and joined the Clan to escape a disaster.",
        [BackstoryType.Disgraced] = "m_c joined the Clan after falling from grace in their previous home.",
    };
    
    // For Backstory types, each flag should have it's own meaning, 
    // * and that meaning should not be effected by any other flags *. 
    // For example, OtherClan *always* means the cat was originally from another clan, no matter what the other
    // tags say. OtherClan & HalfClan would mean the cat lived in the Other Clan for some period of time. 
    // Try to keep the number of tags here minimal. The maximum amount is 64. Tags should be general things that can
    // be referenced in events/thoughts/ext. If more detailed flavor text is desired, pass your own short and long
    // text. 
    [Flags]
    public enum BackstoryType: UInt64
    {
        // If the cat is not in the clan, this is included. 
        NotInClan = 1 << 0, 
        // For cats who are founders of the clan - ie, the starter cats. 
        Founder = 1 << 1, 
        // Cats who were born in the MAIN CLAN, with both parents from that clan.
        Clanborn = 1 << 2,
        // Cats from another clan. 
        OtherClan = 1 << 3,
        // Cats who were refugees
        Refugee = 1 << 4,
        // For cat who left due to some disgrace
        Disgraced = 1 << 5,
        // Cats who were rogues
        Rogue = 1 << 6,
        // Cats who were loners
        Loner = 1 << 7,
        // Cats who were healers
        Healer = 1 << 8,
        // Cats who were kitty-pets
        Kittypet = 1 << 9,
        // Cats where were abandoned as kittens
        Abandoned = 1 << 10,
        // For cats who were orphaned as young cats
        Orphaned = 1 << 11,
        // Cats who joined as kittens
        JoinedAsKit = 1 << 12,
        // For kits with parents in different clans
        HalfClan = 1 << 13,
        // For kits with one clan parents, and one non-clan parents
        HalfOutsider = 1 << 14
    }

    [JsonInclude]
    public BackstoryType types { get; private set; } = BackstoryType.Clanborn;
    [JsonInclude]
    public string longText { get; private set; } = string.Empty;
    [JsonInclude]
    public string shortText { get; private set; } = string.Empty;

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal Backstory() { }

    public Backstory(BackstoryType types)
    {
        GetDefaultText(types, out string defaultLongText, out string defaultShortText);

        this.types = types;
        this.longText = defaultLongText;
        this.shortText = defaultShortText;

    }

    public Backstory(BackstoryType types, string longText, string shortText)
    {
        this.types = types;
        this.longText = longText;
        this.shortText = shortText;
    }

    private static void GetDefaultText(BackstoryType types, out string longText, out string shortText)
    {
        longText = "No long-text given";
        shortText = "No short-text given";
    }
    
}