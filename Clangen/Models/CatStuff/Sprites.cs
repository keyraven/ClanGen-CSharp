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
    public static int spriteSize = 50;
    
    public static Dictionary<string, SKBitmap> lineart { get; set; } = new();
    public static Dictionary<string, SKBitmap> colorPattern { get; set; } = new();
    public static Dictionary<string, SKBitmap> eye1 { get; set; } = new();
    public static Dictionary<string, SKBitmap> eye2 { get; set; } = new();
    public static Dictionary<string, SKBitmap> whitePatches { get; set; } = new();
    public static Dictionary<string, SKBitmap> tortiePattern { get; set; } = new();
    public static Dictionary<string, SKBitmap> scars { get; set; } = new(); 
    public static Dictionary<string, SKBitmap> accessories { get; set; } = new();
    public static Dictionary<string, SKBitmap> fadedCats { get; set; } = new();
    public static Dictionary<string, SKBitmap> shading { get; set; } = new();

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
    private static Dictionary<string, SKBitmap> CreateGroup(SKBitmap spritesheet, string name, int xOffset = 0, 
        int yOffset = 0, int spritesX = 3, int spritesY = 7 )
    {
        Dictionary<string, SKBitmap> output = new();
        int groupXOfs = xOffset * spritesX * spriteSize;
        int groupYOfs = yOffset * spritesY * spriteSize;
        
        int i = 0;
        for (int row = 0; row < spritesY; row++)
        {
            for (int col = 0; col < spritesX; col++)
            {
                // Create new SkBitMap for sprite
                SKBitmap sprite = new SKBitmap(spriteSize, spriteSize);
                
                //Extract the area of interest
                SKRect sourceRect = SKRect.Create(groupXOfs + (row * spriteSize), groupYOfs + (col * spriteSize), 
                    spriteSize, spriteSize);
                SKRect desRect = SKRect.Create(sprite.Width, sprite.Width);

                using (SKCanvas canvas = new SKCanvas(sprite))
                {
                    canvas.Clear(SKColors.Transparent);
                    canvas.DrawBitmap(spritesheet, sourceRect, desRect);
                }

                output[$"{name}{i}"] = sprite;
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
    private static Dictionary<string, SKBitmap> ParseSpritesheet(string path, List<List<string>> groupNames, int spritesX = 3, int spritesY = 7 )
    {
        
        

        SKBitmap spritesheet = SKBitmap.Decode(path);
        Dictionary<string, SKBitmap> output = new();
        int rowNumber = groupNames.Count;
        
        for (int row = 0; row < rowNumber; row++)
        {
            int colNumber = groupNames[row].Count;
            for (int col = 0; col < colNumber; col++)
            {
                
                Console.WriteLine($"Starting Group {groupNames[row][col]}");
                var group = CreateGroup(spritesheet, groupNames[row][col], col, row, spritesX, spritesY);
                Console.WriteLine("End Group");
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
        
        Console.WriteLine("Starting White Patches");
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
        Console.WriteLine("Ending White Patches");
        
        // Single Color
        


    }
}