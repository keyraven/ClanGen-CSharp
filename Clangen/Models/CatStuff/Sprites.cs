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
        

    }
}