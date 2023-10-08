using System;
using System.Collections.Generic;
using System.Diagnostics;
using SkiaSharp;
namespace Clangen.Models.CatStuff;

/// <summary>
/// Holds sprites and generates sprites
/// </summary>
public static class Sprites
{
    private static int spriteSize = 50;
    
    private static Dictionary<string, SKImage> lineart { get; set; } = new();
    private static Dictionary<string, SKImage> colorPattern { get; set; } = new();
    private static Dictionary<string, SKImage> eye1 { get; set; } = new();
    private static Dictionary<string, SKImage> eye2 { get; set; } = new();
    private static Dictionary<string, SKImage> whitePatches { get; set; } = new();
    private static Dictionary<string, SKImage> tortiePattern { get; set; } = new();
    private static Dictionary<string, SKImage> scars { get; set; } = new();
    private static Dictionary<string, SKImage> skins { get; set; } = new();
    private static Dictionary<string, SKImage> accessories { get; set; } = new();
    private static Dictionary<string, SKImage> fadedCats { get; set; } = new();
    private static Dictionary<string, SKImage> shading { get; set; } = new();

    //TODO - something to store the fading effect in. 
    //TODO - something to store tints in. 

    /// <summary>
    /// Generates a single "group" of sprites with the same name, but with different
    /// {i} pose-type. 
    /// </summary>
    /// <param name="spritesheet"></param>
    /// <param name="name"></param>
    /// <param name="xOffset"></param>
    /// <param name="yOffset"></param>
    /// <param name="spritesX"></param>
    /// <param name="spritesY"></param>
    /// <returns></returns>
    private static Dictionary<string, SKImage> CreateGroup(SKImage spritesheet, string name, int xOffset = 0, 
        int yOffset = 0, int spritesX = 3, int spritesY = 7 )
    {
        Dictionary<string, SKImage> output = new();
        int groupXOfs = xOffset * spritesX * spriteSize;
        int groupYOfs = yOffset * spritesY * spriteSize;
        
        int i = 0;
        for (int row = 0; row < spritesY; row++)
        {
            for (int col = 0; col < spritesX; col++)
            {
                //
                SKRectI sourceRect = SKRectI.Create(groupXOfs + (row * spriteSize), groupYOfs + (col * spriteSize), 
                    spriteSize, spriteSize);

                output[$"{name}{i}"] = spritesheet.Subset(sourceRect);
                i++;
            }
        }

        return output;
    }

    /// <summary>
    /// Divides up an entire spritesheet into smaller, named chunks.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="groupNames"> List of list of sub-group names. Each list is a row of sprite groups. </param>
    /// <param name="spritesX"> Number of rows in a sprite-group </param>
    /// <param name="spritesY"> Number of rows in a sprite-group </param>
    /// <returns></returns>
    private static Dictionary<string, SKImage> ParseSpritesheet(string path, List<List<string>> groupNames, int spritesX = 3, int spritesY = 7 )
    {
        
        SKImage spritesheet = SKImage.FromBitmap(SKBitmap.Decode(path));
        Dictionary<string, SKImage> output = new();
        int rowNumber = groupNames.Count;
        
        for (int row = 0; row < rowNumber; row++)
        {
            int colNumber = groupNames[row].Count;
            for (int col = 0; col < colNumber; col++)
            {
                
                var group = CreateGroup(spritesheet, groupNames[row][col], col, row, spritesX, spritesY);
                output.AddRange(group);
            }
        }

        return output;
    }
    
    public static void LoadSprites()
    {
        SKBitmap spritesheet;
        string basepath = "Resources/sprites/";
        List<List<string>> groupNames;
        
        // LineArt
        groupNames = new()
        {
            new() {"alive"}
        };
        lineart.AddRange(ParseSpritesheet($"{basepath}lineart.png", groupNames));
        
        groupNames = new()
        {
            new() {"dead"}
        };
        lineart.AddRange(ParseSpritesheet($"{basepath}lineartdead.png", groupNames));
        
        groupNames = new()
        {
            new() {"df"}
        };
        lineart.AddRange(ParseSpritesheet($"{basepath}lineartdf.png", groupNames));
        
        //Shading
        groupNames = new()
        {
            new() {"shade"}
        };
        shading.AddRange(ParseSpritesheet($"{basepath}shadersnewwhite.png", groupNames));
        groupNames = new()
        {
            new() {"light"}
        };
        shading.AddRange(ParseSpritesheet($"{basepath}lightingnew.png", groupNames));
        
        // White Patches 
        groupNames = new()
        {
            new() {"FULLWHITE", "ANY", "TUXEDO", "LITTLE", "COLOURPOINT", "VAN", "ANYTWO",
                "MOON", "PHANTOM", "POWDER", "BLEACHED", "SAVANNAH", "FADESPOTS", "PEBBLESHINE"},
            new() {"EXTRA", "ONEEAR", "BROKEN", "LIGHTTUXEDO", "BUZZARDFANG", "RAGDOLL", 
                "LIGHTSONG", "VITILIGO", "BLACKSTAR", "PIEBALD", "CURVED", "PETAL", "SHIBAINU", "OWL"},
            new() {"TIP", "FANCY", "FRECKLES", "RINGTAIL", "HALFFACE", "PANTSTWO", "GOATEE", "VITILIGOTWO",
                "PAWS", "MITAINE", "BROKENBLAZE", "SCOURGE", "DIVA", "BEARD"},
            new() {"TAIL", "BLAZE", "PRINCE", "BIB", "VEE", "UNDERS", "HONEY",
                "FAROFA", "DAMIEN", "MISTER", "BELLY", "TAILTIP", "TOES", "TOPCOVER"},
            new() {"APRON", "CAPSADDLE", "MASKMANTLE", "SQUEAKS", "STAR", "TOESTAIL", "RAVENPAW",
                "PANTS", "REVERSEPANTS", "SKUNK", "KARPATI", "HALFWHITE", "APPALOOSA", "DAPPLEPAW"},
            new() {"HEART", "LILTWO", "GLASS", "MOORISH", "SEPIAPOINT", "MINKPOINT", "SEALPOINT",
                "MAO", "LUNA", "CHESTSPECK", "WINGS", "PAINTED", "HEARTTWO", "WOODPECKER"},
            new () {"BOOTS", "MISS", "COW", "COWTWO", "BUB", "BOWTIE", "MUSTACHE", "REVERSEHEART",
                "SPARROW", "VEST", "LOVEBUG", "TRIXIE", "SAMMY", "SPARKLE"},
            new() {"RIGHTEAR", "LEFTEAR", "ESTRELLA", "SHOOTINGSTAR", "EYESPOT", "REVERSEEYE",
                "FADEBELLY", "FRONT", "BLOSSOMSTEP", "PEBBLE", "TAILTWO", "BUDDY", "BACKSPOT", "EYEBAGS"},
            new() {"BULLSEYE", "FINN", "DIGIT", "KROPKA", "FCTWO", "FCONE", "MIA", "SCAR", "BUSTER", "SMOKEY"}
        };
        whitePatches.AddRange(ParseSpritesheet($"{basepath}whitepatches.png", groupNames));
        
        // PELTS
        groupNames = new()
        {
            new() {"WHITE", "PALEGREY", "SILVER", "GREY", "DARKGREY", "GHOST", "BLACK"},
            new() {"CREAM", "PALEGINGER", "GOLDEN", "GINGER", "DARKGINGER", "SIENNA"},
            new() {"LIGHTBROWN", "LILAC", "BROWN", "GOLDEN-BROWN", "DARKBROWN", "CHOCOLATE"}
        };

        List<string> pelts = new()
        {
            "single", "tabby", "marbled", "rosette", "smoke", "ticked", "speckled", "bengal", 
            "mackerel", "classic", "sokoke", "agouti", "singlestripe"
        };
        
        foreach (string name in pelts)
        {
            //  Generate a list of names
            List<List<string>> peltNames = new();
            foreach (var sublist in groupNames)
            {
                List<string> newSublist = new();
                foreach (var colorname in sublist)
                {
                    newSublist.Add($"{name}{colorname}");
                }
                peltNames.Add(newSublist);
            }
            
            colorPattern.AddRange(ParseSpritesheet($"{basepath}{name}colours.png", peltNames));
        }
        
        // Torties
        groupNames = new()
        {
            new List<string>() {"ONE", "TWO", "THREE", "FOUR", "REDTAIL", "DELILAH", "HALF", "STREAK", "MASK", "SMOKE"},
            new List<string>() {"MINIMALONE", "MINIMALTWO", "MINIMALTHREE", "MINIMALFOUR", "OREO", "SWOOP", "CHIMERA", "CHEST", "ARMTAIL", "GRUMPYFACE"},
            new List<string>() {"MOTTLED", "SIDEMASK", "EYEDOT", "BANDANA", "PACMAN", "STREAMSTRIKE", "SMUDGED", "DAUB", "EMBER", "BRIE"},
            new List<string>() {"ORIOLE", "ROBIN", "BRINDLE", "PAIGE", "ROSETAIL", "SAFI", "DAPPLENIGHT", "BLANKET", "BELOVED", "BODY"},
            new List<string>() {"SHILOH"}
        };
        tortiePattern.AddRange(ParseSpritesheet($"{basepath}tortiepatchesmasks.png", groupNames));
        
        //Skins
        groupNames = new()
        {
            new() { "BLACK", "RED", "PINK", "DARKBROWN", "BROWN", "LIGHTBROWN" },
            new() { "DARK", "DARKGREY", "GREY", "DARKSALMON", "SALMON", "PEACH" },
            new() { "DARKMARBLED", "MARBLED", "LIGHTMARBLED", "DARKBLUE", "BLUE", "LIGHTBLUE" }
        };
        skins.AddRange(ParseSpritesheet($"{basepath}skin.png", groupNames));
        
        
        // Accessories
        groupNames = new()
        {
            new() { "MAPLE LEAF", "HOLLY", "BLUE BERRIES", "FORGET ME NOTS", "RYE STALK", "LAUREL" },
            new() { "BLUEBELLS", "NETTLE", "POPPY", "LAVENDER", "HERBS", "PETALS" },
            new() {"RED FEATHERS", "BLUE FEATHERS", "JAY FEATHERS", "MOTH WINGS", "CICADA WINGS", "DRY HERBS"},
            new() { "OAK LEAVES", "CATMINT", "MAPLE SEED", "JUNIPER" }
        };
        accessories.AddRange(ParseSpritesheet($"{basepath}medcatherbs.png", groupNames));
        
        groupNames = new()
        {
            new() { "CRIMSON", "BLUE", "YELLOW", "CYAN", "RED", "LIME" },
            new() { "GREEN", "RAINBOW", "BLACK", "SPIKES", "WHITE" },
            new() { "PINK", "PURPLE", "MULTI", "INDIGO" }
        };
        accessories.AddRange(ParseSpritesheet($"{basepath}collars.png", groupNames));
        
        groupNames = new()
        {
            new() { "CRIMSONBELL", "BLUEBELL", "YELLOWBELL", "CYANBELL", "REDBELL", "LIMEBELL" },
            new() { "GREENBELL", "RAINBOWBELL", "BLACKBELL", "SPIKESBELL", "WHITEBELL" },
            new() { "PINKBELL", "PURPLEBELL", "MULTIBELL", "INDIGOBELL" }
        };
        accessories.AddRange(ParseSpritesheet($"{basepath}bellcollars.png", groupNames));
        
        groupNames = new()
        {
            new() { "CRIMSONBOW", "BLUEBOW", "YELLOWBOW", "CYANBOW", "REDBOW", "LIMEBOW" },
            new() { "GREENBOW", "RAINBOWBOW", "BLACKBOW", "SPIKESBOW", "WHITEBOW" },
            new() { "PINKBOW", "PURPLEBOW", "MULTIBOW", "INDIGOBOW" }
        };
        accessories.AddRange(ParseSpritesheet($"{basepath}bowcollars.png", groupNames));
        
        groupNames = new()
        {
            new() { "CRIMSONNYLON", "BLUENYLON", "YELLOWNYLON", "CYANNYLON", "REDNYLON", "LIMENYLON" },
            new() { "GREENNYLON", "RAINBOWNYLON", "BLACKNYLON", "SPIKESNYLON", "WHITENYLON" },
            new() { "PINKNYLON", "PURPLENYLON", "MULTINYLON", "INDIGONYLON" }
        };
        accessories.AddRange(ParseSpritesheet($"{basepath}nyloncollars.png", groupNames));
        
        // TODO - SCARS
    }
    
    
    public static SKImage GenerateSprite(Cat cat)
    {
        SKBitmap sprite = new SKBitmap(spriteSize, spriteSize);
        
        //Placeholder for testing
        string spriteNumber = "1";
        
        using (SKCanvas canvas = new SKCanvas(sprite))
        {
            canvas.Clear(SKColors.Transparent);
            
            canvas.DrawImage(colorPattern[$"singleWHITE{spriteNumber}"], 0, 0);
            canvas.DrawImage(lineart[$"alive{spriteNumber}"], 0, 0);
            
        }

        return SKImage.FromBitmap(sprite);
    }
    
}