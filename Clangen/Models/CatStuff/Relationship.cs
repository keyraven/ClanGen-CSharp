using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clangen.Models.CatStuff;


public class Relationship
{
    
    // Various min and max values. Public, so they can be used 
    // in other functions. 
    public const int MinRomantic = 0;
    public const int MaxRomantic = 100;
    public const int MinSentiment = -100;
    public const int MaxSentiment = 100;
    public const int MinLoyalty = -100;
    public const int MaxLoyalty = 100;
    
    
    // Difference from python version --- NOTE
    // Relationship catFrom and catTo are now stored as IDs.
    public string catFrom { get; set; }
    public string catTo { get; set; }
    
    public List<string> log { get; set; }
    
    private int _romantic;
    private int _sentiment;
    private int _loyalty;
    
    // You may notice this is significantly simplified. That is intentional. 
    // Although a variety of values is nice, they tend towards max-out "good"
    // or maxed-out "bad", due to the complex add functions.
    // Only these three concepts are ever really used to determine behavior. 
    // And three is a better number for the brain. 7 is too much to get your brain around, and
    // too much detail to build a story around, I think. And most of the previous concepts
    // were non-orthogonal (thus the complex adds). These are designed to be mostly independent.  
    // ( 1 ) romantic is the same as before. 
    // ( 2 ) platonicLike and dislike have now been compressed into a single value, sentiment,
    //       with negative values for dislike and positive for like. 
    // ( 3 ) loyalty replaces trust and respect. It is similar, but more actionable, and can go negative
    //       for distrust/disrespect.
    // Jealousy has been removed. Comfort is too close to Like, so it gets folded in. They were never really used much. 
    // - keyraven
    
    
    /// <summary>
    /// A catFrom's romantic interest in catTo. 0 is none, 100 is max. 
    /// </summary>
    public int romantic
    {
        get { return _romantic; }
        set { _romantic = AdjustToRange(value, MinRomantic, MaxRomantic); }
    }
    
    /// <summary>
    /// Generally, how catFrom feels about catTo. Ranges from -100 to 100.
    /// Negative values indicate negative feelings (hate/enemies), and positive values positive feelings (friends). 
    /// </summary>
    public int sentiment
    {
        get { return _sentiment; }
        set { _sentiment = AdjustToRange(value, MinSentiment, MaxSentiment); }
    }
    
    /// <summary>
    /// How loyal catFrom is to catTo. Ranges from -100 to 100.  At negative values,
    /// catFrom may be willing to betray catTo. At high positive values,
    /// catFrom will follow catTo to the ends of the earth, and trusts them with their life.
    /// </summary>
    public int loyalty
    {
        get { return _loyalty; }
        set { _loyalty = AdjustToRange(value, MinLoyalty, MaxLoyalty); }
    }
    
    public Relationship(string catFrom, string catTo, int romantic = 0, int sentiment = 0, int loyalty = 0, List<string>? log = null)
    {
        log ??= new();

        this.catFrom = catFrom;
        this.catTo = catTo;
        this.romantic = romantic;
        this.sentiment = sentiment;
        this.loyalty = loyalty;
        this.log = log;
    }
    
    private static int AdjustToRange(int value, int minVal, int maxVal)
    {
        if (value > maxVal) { value = maxVal; }
        else if (value < minVal) { value = minVal; }

        return value;
    }
}
