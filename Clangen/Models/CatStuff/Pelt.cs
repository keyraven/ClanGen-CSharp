using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clangen.Models.CatStuff;

public class Pelt
{
    // Needed
    public string peltColor { get; private set; } = "WHITE";
    public string peltPattern { get; private set; } = "single";
    public string eyeColor { get; private set; } = "BLUE";
    public string skin { get; private set; } = "PINK";
    public string kittenSprite { get; private set; } = "0";
    public string adolSprite { get; private set; } = "3";
    public string adultSprite { get; private set; } = "6";
    public string seniorSprite { get; private set; } = "9";
    public string paraAdultSprite { get; private set; } = "12";
    public string peltLength { get; private set; }= "short";
    public List<string> scars { get; set; } = new();

    // Optional/Nullable
    public string? whitePatches { get; private set; }
    public string? accessory { get; private set; }
    public string? eyeColor2 { get; private set; }
    public string? tint { get; private set; }
    public string? whitePatchesTint { get; private set; }
    
    
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
    
    public Pelt(List<Cat> parents)
    {
        
    }

    public Pelt(Cat par1, Cat? par2 = null)
    {
        
    }
}
