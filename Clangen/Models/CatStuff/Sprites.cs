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
    private static Dictionary<string, SKImage> tortiePatches { get; set; } = new();
    private static Dictionary<string, SKImage> scars { get; set; } = new();
    private static Dictionary<string, SKImage> skins { get; set; } = new();
    private static Dictionary<string, SKImage> accessories { get; set; } = new();
    private static Dictionary<string, SKImage> fadedCats { get; set; } = new();
    private static Dictionary<string, SKImage> shading { get; set; } = new();
    private static Dictionary<string, byte[]?> peltTints { get; set; } = new();
    private static Dictionary<string, byte[]?> whitePatchTint { get; set; } = new();
    
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
        for (int col = 0; col < spritesY; col++)
        {
            for (int row = 0; row < spritesX; row++)
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
        tortiePatches.AddRange(ParseSpritesheet($"{basepath}tortiepatchesmasks.png", groupNames));
        
        //Skins
        groupNames = new()
        {
            new() { "BLACK", "RED", "PINK", "DARKBROWN", "BROWN", "LIGHTBROWN" },
            new() { "DARK", "DARKGREY", "GREY", "DARKSALMON", "SALMON", "PEACH" },
            new() { "DARKMARBLED", "MARBLED", "LIGHTMARBLED", "DARKBLUE", "BLUE", "LIGHTBLUE" }
        };
        skins.AddRange(ParseSpritesheet($"{basepath}skin.png", groupNames));
        
        // Eye
        groupNames = new()
        {
            new()
            {
                "YELLOW", "AMBER", "HAZEL", "PALEGREEN", "GREEN", "BLUE",
                "DARKBLUE", "GREY", "CYAN", "EMERALD", "HEATHERBLUE", "SUNLITICE"
            },
            new()
            {
                "COPPER", "SAGE", "COBALT", "PALEBLUE", "BRONZE", "SILVER",
                "PALEYELLOW", "GOLD", "GREENYELLOW"
            }
        };
        eye1.AddRange(ParseSpritesheet($"{basepath}eyes.png", groupNames));
        eye2.AddRange(ParseSpritesheet($"{basepath}eyes2.png", groupNames));
        
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
        
        //TINTS
        // TODO -- LOAD TINTS FROM FILE
        whitePatchTint = new Dictionary<string, byte[]?>()
        {
            ["darkcream"] = new byte[] {236, 229, 208},
            ["cream"] = new byte[] {247, 241, 225},
            ["offwhite"] = new byte[] {238, 249, 252},
            ["gray"] = new byte[] {208, 225, 229},
            ["pink"] = new byte[] {254, 248, 249},
            ["black"] = new byte[] {255, 0, 0},
            ["none"] = null
        };
        
        peltTints = new Dictionary<string, byte[]?>()
        {
            ["pink"] = new byte[] {253, 237, 237},
            ["gray"] = new byte[] {225, 225, 225},
            ["red"] = new byte[] {248, 226, 228},
            ["black"] = new byte[] {195, 195, 195},
            ["orange"] = new byte[] {255, 247, 235},
            ["yellow"] = new byte[] {250, 248, 225},
            ["purple"] = new byte[] {235, 225, 244},
            ["blue"] = new byte[] {218, 237, 245},
            ["none"] = null
        };
        
        // TODO - SCARS
    }

    /// <summary>
    /// Applies a tint the source image. Does not effect transparency. 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="color"></param>
    /// <param name="blendMode"></param>
    /// <returns></returns>
    private static SKBitmap ApplyTint(SKImage source, SKBlendMode blendMode)
    {
        // this is almost certainly the worst way to do this. 
        SKColor color = new SKColor(0, 0, 0, 0);
        SKBitmap output = SKBitmap.FromImage(source);
        using (SKCanvas canvas = new SKCanvas(output))
        {
            SKPaint paint = new SKPaint()
            {
                ColorFilter = SKColorFilter.CreateBlendMode(SKColors.Aqua, blendMode),
                BlendMode = SKBlendMode.SrcATop
            };
            
            canvas.DrawImage(source, 0, 0, paint: paint);
        }

        return output;
    }

    public static SKImage GenerateSprite(Cat cat)
    {
        SKBitmap sprite = new SKBitmap(spriteSize, spriteSize);
        
        //Placeholder for testing
        string spriteNumber = cat.pelt.GetSpriteNumber(cat.age, cat.canWork);
        
        // TODO - Figure out a nice way to do tints. 
        try
        {
            using (SKCanvas canvas = new SKCanvas(sprite))
            {
                canvas.Clear(SKColors.Transparent);
                
                //PATTERN
                canvas.DrawImage(colorPattern[$"{cat.pelt.peltPattern}{cat.pelt.peltColor}{spriteNumber}"], 0, 0);
                
                
                //WHITE PATCHES
                if (cat.pelt.whitePatches != null)
                {
                    
                    canvas.DrawImage(whitePatches[$"{cat.pelt.whitePatches}{spriteNumber}"], 0, 0);
                }
                
                // Eyes
                canvas.DrawImage(eye1[$"{cat.pelt.eyeColor}{spriteNumber}"], 0, 0);
                if (cat.pelt.eyeColor2 != null)
                {
                    canvas.DrawImage(eye2[$"{cat.pelt.eyeColor2}{spriteNumber}"], 0, 0);
                }
                
                // Lineart
                if (!cat.dead)
                {
                    canvas.DrawImage(lineart[$"alive{spriteNumber}"], 0, 0);
                }
                //else if (cat.darkForest)
                //{
                //    canvas.DrawImage(lineart[$"df{spriteNumber}"], 0, 0);
                //}
                else
                {
                    canvas.DrawImage(lineart[$"dead{spriteNumber}"], 0, 0);
                }
                
                // Accessory
                if (cat.pelt.accessory != null)
                {
                    canvas.DrawImage(accessories[$"{cat.pelt.accessory}{spriteNumber}"], 0, 0);
                }
                
            }
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("ERROR WITH SPRITE");
        }
        
        return SKImage.FromBitmap(sprite);
    }
    
}

