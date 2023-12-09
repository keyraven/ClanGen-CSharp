using System;
using System.Collections.Generic;

namespace Clangen.Models.CatStuff;

public class Pelt
{
    private static readonly IReadOnlyList<string> AllWhitePatches = new List<string>()
    {
        "FULLWHITE", "ANY", "TUXEDO", "LITTLE", "COLOURPOINT", "VAN", "ANYTWO",
        "MOON", "PHANTOM", "POWDER", "BLEACHED", "SAVANNAH", "FADESPOTS", "PEBBLESHINE",
        "EXTRA", "ONEEAR", "BROKEN", "LIGHTTUXEDO", "BUZZARDFANG", "RAGDOLL",
        "LIGHTSONG", "VITILIGO", "BLACKSTAR", "PIEBALD", "CURVED", "PETAL", "SHIBAINU", "OWL",
        "TIP", "FANCY", "FRECKLES", "RINGTAIL", "HALFFACE", "PANTSTWO", "GOATEE", "VITILIGOTWO",
        "PAWS", "MITAINE", "BROKENBLAZE", "SCOURGE", "DIVA", "BEARD",
        "TAIL", "BLAZE", "PRINCE", "BIB", "VEE", "UNDERS", "HONEY",
        "FAROFA", "DAMIEN", "MISTER", "BELLY", "TAILTIP", "TOES", "TOPCOVER",
        "APRON", "CAPSADDLE", "MASKMANTLE", "SQUEAKS", "STAR", "TOESTAIL", "RAVENPAW",
        "PANTS", "REVERSEPANTS", "SKUNK", "KARPATI", "HALFWHITE", "APPALOOSA", "DAPPLEPAW",
        "HEART", "LILTWO", "GLASS", "MOORISH", "SEPIAPOINT", "MINKPOINT", "SEALPOINT",
        "MAO", "LUNA", "CHESTSPECK", "WINGS", "PAINTED", "HEARTTWO", "WOODPECKER",
        "BOOTS", "MISS", "COW", "COWTWO", "BUB", "BOWTIE", "MUSTACHE", "REVERSEHEART",
        "SPARROW", "VEST", "LOVEBUG", "TRIXIE", "SAMMY", "SPARKLE",
        "RIGHTEAR", "LEFTEAR", "ESTRELLA", "SHOOTINGSTAR", "EYESPOT", "REVERSEEYE",
        "FADEBELLY", "FRONT", "BLOSSOMSTEP", "PEBBLE", "TAILTWO", "BUDDY", "BACKSPOT", "EYEBAGS",
        "BULLSEYE", "FINN", "DIGIT", "KROPKA", "FCTWO", "FCONE", "MIA", "SCAR", "BUSTER", "SMOKEY"
    };
    
    private static readonly IReadOnlyList<string> AllColorNames = new List<string>()
    {
        "WHITE", "PALEGREY", "SILVER", "GREY", "DARKGREY", "GHOST", "BLACK",
        "CREAM", "PALEGINGER", "GOLDEN", "GINGER", "DARKGINGER", "SIENNA",
        "LIGHTBROWN", "LILAC", "BROWN", "GOLDEN-BROWN", "DARKBROWN", "CHOCOLATE"
    };

    private static readonly IReadOnlyList<string> AllPeltPatterns = new List<string>()
    {
        "single", "tabby", "marbled", "rosette", "smoke", "ticked", "speckled", "bengal", 
        "mackerel", "classic", "sokoke", "agouti", "singlestripe"
    };
    
    private static readonly IReadOnlyList<string> AllEyeColors = new List<string>()
    {
        "YELLOW", "AMBER", "HAZEL", "PALEGREEN", "GREEN", "BLUE",
        "DARKBLUE", "GREY", "CYAN", "EMERALD", "HEATHERBLUE", "SUNLITICE",
        "COPPER", "SAGE", "COBALT", "PALEBLUE", "BRONZE", "SILVER",
        "PALEYELLOW", "GOLD", "GREENYELLOW"
    };

    private static readonly IReadOnlyList<string> AllTortiePatches = new List<string>()
    {
        "ONE", "TWO", "THREE", "FOUR", "REDTAIL", "DELILAH", "HALF", "STREAK", "MASK", "SMOKE",
        "MINIMALONE", "MINIMALTWO", "MINIMALTHREE", "MINIMALFOUR", "OREO", "SWOOP", "CHIMERA", "CHEST", 
        "ARMTAIL", "GRUMPYFACE", "MOTTLED", "SIDEMASK", "EYEDOT", "BANDANA", "PACMAN", "STREAMSTRIKE", "SMUDGED", 
        "DAUB", "EMBER", "BRIE", "ORIOLE", "ROBIN", "BRINDLE", "PAIGE", "ROSETAIL", "SAFI", "DAPPLENIGHT", 
        "BLANKET", "BELOVED", "BODY", "SHILOH"
    };

    private static readonly IReadOnlyList<string> AllPeltLengths = new List<string>()
    {
        "SHORT", "MEDIUM", "LONG"
    };
    
    private const string newbornSprite = "20";
    private const string paraYoungSprite = "17";
    private const string sickYoungSprite = "19";
    private const string sickAdultSprite = "20";
    
    
    // Needed
    public string peltColor { get; private set; } = AllColorNames[0];
    public string peltPattern { get; private set; } = AllPeltPatterns[0];
    public string eyeColor { get; private set; } = AllEyeColors[0];
    public string skin { get; private set; } = "PINK";
    
    public string kittenSprite { get; private set; } = "0";
    public string adolSprite { get; private set; } = "3";
    public string adultSprite { get; private set; } = "6";
    public string seniorSprite { get; private set; } = "9";
    public string paraAdultSprite { get; private set; } = "12";
    public string peltLength { get; private set; }= "short";
    public List<string> scars { get; set; } = new();
    public bool paralyzed { get; set; } = false;
    public string tint { get; private set; } = "none";
    public string whitePatchesTint { get; private set; } = "none";

    /// <summary>
    /// The pattern inside the tortie patches. IE, Tabby, Single, ext
    /// </summary>
    public string tortiePattern { get; private set; } = "single";

    /// <summary>
    /// The color of the tortie patches. 
    /// </summary>
    public string tortieColor { get; private set; } = "white";

    // Optional/Nullable
    public string? whitePatches { get; private set; } = null;
    public string? points { get; private set; } = null;
    public string? vitiligo { get; private set; } = null;
    public string? accessory { get; private set; } = null;
    public string? eyeColor2 { get; private set; } = null;
    
    /// <summary>
    /// The shape of the tortie patches --> The mask
    /// Non-null tortiePatches marks the cat as a tortie
    /// </summary>
    public string? tortiePatches { get; private set; }


    public static Pelt GenerateRandomPelt(Cat.CatSex sex, List<Cat>? parents = null)
    {
        Pelt newPelt = new Pelt();
        
        // DETERMINE PELT LENGTH
        newPelt.peltLength = Utilities.ChoseRandom((IList<string>)AllPeltLengths);
        if (newPelt.peltLength  == "LONG")
        {
            newPelt.adultSprite = Utilities.RandomInt(9, 11).ToString();
            newPelt.paraAdultSprite = "13";
        }
        else
        {
            newPelt.adultSprite = Utilities.RandomInt(6, 8).ToString();
            newPelt.paraAdultSprite = "12";
        }
        
        newPelt.seniorSprite = Utilities.RandomInt(13, 15).ToString();
        newPelt.kittenSprite = Utilities.RandomInt(0, 2).ToString();
        newPelt.adolSprite = Utilities.RandomInt(3, 5).ToString();
        
        newPelt.peltPattern = Utilities.ChoseRandom((IList<string>)AllPeltPatterns);
        newPelt.peltColor = Utilities.ChoseRandom((IList<string>)AllColorNames);
        newPelt.eyeColor = Utilities.ChoseRandom((IList<string>)AllEyeColors);
        
        if (Utilities.InverseChanceRoll(3))
        {
            newPelt.whitePatches = Utilities.ChoseRandom((IList<string>)AllWhitePatches);
        }
        
        // Determine tortie. TODO- add sex dependence
        if (Utilities.InverseChanceRoll(4))
        {
            newPelt.tortiePatches = Utilities.ChoseRandom((IList<string>)AllTortiePatches);
            newPelt.tortieColor = Utilities.ChoseRandom((IList<string>)AllColorNames);
            newPelt.tortiePattern = Utilities.ChoseRandom((IList<string>)AllPeltPatterns);
        }
        

        return newPelt;
    }
    
    public string GetSpriteNumber(Cat.CatAge age, bool canWork)
    {
        switch (age)
        {
            case Cat.CatAge.Newborn:
                return newbornSprite;
            
            case Cat.CatAge.Kitten:
                if (!canWork) { return sickYoungSprite; }
                return paralyzed ? paraYoungSprite : kittenSprite;
            
            case Cat.CatAge.Adolescent:
                if (!canWork) { return sickYoungSprite; }
                return paralyzed ? paraYoungSprite : adolSprite;
            
            case Cat.CatAge.YoungAdult:
            case Cat.CatAge.Adult:
            case Cat.CatAge.SeniorAdult:
                if (!canWork) { return sickAdultSprite; }
                return paralyzed ? paraAdultSprite : adultSprite;
            
            case (Cat.CatAge.Senior):
                if (!canWork) { return sickAdultSprite; }
                return paralyzed ? paraAdultSprite : seniorSprite;
            
            default:
                if (!canWork) { return sickAdultSprite; }
                return paralyzed ? paraAdultSprite : adultSprite;
        }
    }

    /// <summary>
    /// Returns the name of the pattern
    /// </summary>
    /// <returns></returns>
    public string GetPatternName()
    {
        if (peltPattern == "single")
        {
            if (whitePatches is null && points is null)
            {
                return "self-solid";
            }

            return "two-color";
        }

        if (tortiePatches != null)
        {
            if (whitePatches is null && points is null)
            {
                return "tortoiseshell";
            }

            return "calico";
        }

        return peltPattern;
    }
}
