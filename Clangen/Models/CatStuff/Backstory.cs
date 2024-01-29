using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Clangen.Models.CatStuff;

public class Backstory
{
    private IReadOnlyDictionary<BackstoryType, string> _defaultShortText = new Dictionary<BackstoryType, string>()
    {
        
    };
    
    private IReadOnlyDictionary<BackstoryType, string> _defaultLongText = new Dictionary<BackstoryType, string>()
    {
        [BackstoryType.Founder] = "m_c is one of the founding members of the Clan.",
        [BackstoryType.Clanborn] = "m_c was born into the Clan."
    };
    
    [Flags]
    public enum BackstoryType: UInt64
    {
        Founder = 0,
        Clanborn = 1 << 0,
        OtherClan = 1 << 1,
        Refugee = 1 << 2,
        Rogue = 1 << 3,
        Loner = 1 << 4,
        Healer = 1 << 5,
        Kittypet = 1 << 6,
        Abandoned = 1 << 7,
        NotInClan = 1 << 8, 
        JoinedAsKit = 1 << 9,
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