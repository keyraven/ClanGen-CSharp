using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clangen.Models.CatStuff;

public partial class Pelt
{
    public enum PeltColor
    {
        White, PaleGray, Silver, Gray, DarkGray, Ghost, Black, Cream, PaleGinger, Golden, Ginger, 
        DarkGinger, Sienna, LightBrown, Lilac, Brown, GoldenBrown, DarkBrown, Chocolate
        
    }

    public enum TortiePatterns
    {
        One, Two, Three, Four, Redtail, Delilah, MinimalOne, MinimalThree, MinimalFour, Half, Oreo, Swoop, 
        Mottled, Sidemask, EyeDot, Bandana, Pacman, Streamstrike, Oriole, Chimera, Daub, Ember, Blanket, 
        Robin, Brindle, Paige, Rosetail, Safi, Smudged, Dapplenight, Streak, Mask, Chest, Armtail, Smoke, 
        GrumpyFace, Brie, Beloved
    }

    public enum PeltLength
    {
        Short, Medium, Long
    }

    public enum EyeColor
    {
        Yellow, Amber, Hazel, PaleGreen, Green, Blue, DarkBlue, Gray, Cyan, Emerald, PaleBlue, PaleYellow, 
        Gold, HeatherBlue, Copper, Sage, Cobalt, SunlitIce, GreenYellow, Bronze, Silver
    }

    public enum Scars
    {
        
        One, Two, Three, TailScar, Snout, Cheek, Side, Throat, TailBase, Belly, LegBite, NeckBike, Face, ManLeg, 
        Brightheart, ManTail, Bridge, RightBlind, LeftBlind, BothBlind, BeakCheek, BeakLower, CatBite, QuillChunk,
        QuillScratch, LeftEar, RightEar, NoTail, HalfTail, NoPaw, NoLeftEar, NoRightEar, NoEar, Snake, ToeTrap, 
        BurnPaws, BurnTail, BurnBelly, BurnRump, FrostFace, FrostTail, FrostMitt, FrostSock
    }

    public enum Accessories
    {
        MapleLeaf, Holly, BlueBerries, ForgetMeNots, RyeStalks, Laurel, BlueBells, Nettle, Poppy, Lavender, Herbs,
        Petals, DryHerbs, OakLeaves, Catmint, MapleSeed, Juniper, RedFeathers, BlueFeathers, JayFeathers, MothWings, 
        CicadaWings, CrimsonCollar, BlueCollar, YellowCollar, CyanCollar, RedCollar, LineCollar, GreenCollar,
        RainbowCollar, BlackCollar, SpikeCollar, WhiteCollar, PinkCollar, PurpleCollar, MultiCollar, IndigoCollar, 
        CrimsonBellCollar,  BlueBellCollar, YellowBellCollar, CyanBellCollar, RedBellCollar, LineBellCollar, 
        GreenBellCollar, RainbowBellCollar, BlackBellCollar, SpikeBellCollar, WhiteBellCollar, PinkBellCollar, 
        PurpleBellCollar, MultiBellCollar, IndigoBellCollar, CrimsonBowCollar, BlueBowCollar, YellowBowCollar, 
        CyanBowCollar, RedBowCollar, LineBowCollar, GreenBowCollar, RainbowBowCollar, BlackBowCollar, 
        SpikeBowCollar, WhiteBowCollar, PinkBowCollar, PurpleBowCollar, MultiBowCollar, IndigoBowCollar
    }
    
    
    
}
