using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace Clangen.Models.CatStuff;

public class Pelt
{
    private static readonly IReadOnlyList<string> PeltPattern = new List<string>() { "single", "tabby", "mackeral" }; 
    
    // Needed
    public string peltColor { get; private set; } = "BROWN";
    public string peltPattern { get; private set; } = "tabby";
    public string eyeColor { get; private set; } = "BLUE";
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
    public string whitePatchesTint { get; private set; } = "black";

    // Optional/Nullable
    public string? whitePatches { get; private set; } = "RAVENPAW";
    public string? points { get; private set; }
    public string? vitiligo { get; private set; }
    public string? accessory { get; private set; }
    public string? eyeColor2 { get; private set; }
    
    
    
    /// <summary>
    /// The shape of the tortie patches --> The mask
    /// </summary>
    public string? tortiePatches { get; private set; }
    /// <summary>
    /// The pattern inside the tortie patches. IE, Tabby, Single, ext
    /// </summary>
    public string? tortiePattern { get; private set; }
    /// <summary>
    /// The color of the tortie patches. 
    /// </summary>
    public string? tortieColor { get; private set; }
    
    

    public Pelt(Cat? par1 = null, Cat? par2 = null)
    {
        
    }

    
    public string GetSpriteNumber(Cat.CatAge age, bool canWork)
    {
        switch (age)
        {
            case Cat.CatAge.Newborn:
                return "20";
            
            case Cat.CatAge.Kitten:
                if (!canWork) { return "19"; }
                return paralyzed ? "17" : kittenSprite;
            
            case Cat.CatAge.Adolescent:
                if (!canWork) { return "19"; }
                return paralyzed ? "17" : adolSprite;
            
            case Cat.CatAge.YoungAdult:
            case Cat.CatAge.Adult:
            case Cat.CatAge.SeniorAdult:
                if (!canWork) { return "18"; }
                return paralyzed ? paraAdultSprite : adultSprite;
            
            case (Cat.CatAge.Senior):
                if (!canWork) { return "18"; }
                return paralyzed ? paraAdultSprite : seniorSprite;
            
            default:
                if (!canWork) { return "18"; }
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

        if (peltPattern == "tortie")
        {
            if (whitePatches is null && points is null)
            {
                return "tortoiseshell";
            }

            return "calico";
        }

        return peltPattern;
    }

    private void InitFurLength()
    {
        
    }
    
    private void InitColorPattern()
    {
        
    }
    
    private void InitWhitePatches()
    {
        
    }

    private void InitEyes()
    {
        
    }

    private void InitTints()
    {
        
    }
}
