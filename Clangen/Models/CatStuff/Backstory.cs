using System;

namespace Clangen.Models.CatStuff;

public class Backstory
{
    [Flags]
    public enum BackstoryType
    {
        Founder = 0,
        Clanborn = 1 << 0,
        OtherClan = 1 << 1,
        Refugee = 1 << 2,
        Rogue = 1 << 3,
        Loner = 1 << 4,
        Healer = 1 << 5
    }
    
    public BackstoryType types { get; }
    public string longText { get; }
    public string shortText { get; }
    
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