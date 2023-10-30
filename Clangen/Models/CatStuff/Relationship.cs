using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clangen.Models.CatStuff;


public class Relationship
{
    private const int ValueMin = 0;
    private const int ValueMax = 100;

    // Difference from python version --- NOTE
    // Relationship catFrom and catTo are now stored as private IDs. 
    public string catFrom { get; set; }
    public string catTo { get; set; }
    
    public List<string> log { get; set; }
    
    private int _romanticLove;
    private int _platonicLike;
    private int _dislike;
    private int _admiration;
    private int _comfortable;
    private int _jealousy;
    private int _trust;
    public int romanticLove
    {
        get { return _romanticLove; }
        set { _romanticLove = AdjustToRange(value); }
    }
    public int platonicLike
    {
        get { return _platonicLike; }
        set { _platonicLike = AdjustToRange(value); }
    }
    public int dislike
    {
        get { return _dislike; }
        set { _dislike = AdjustToRange(value); }
    }
    public int admiration
    {
        get { return _admiration; }
        set { _admiration = AdjustToRange(value); }
    }
    public int comfortable
    {
        get { return _comfortable; }
        set { _comfortable = AdjustToRange(value); }
    }
    public int jealousy
    {
        get { return _jealousy; }
        set { _jealousy = AdjustToRange(value); }
    }
    public int trust
    {
        get { return _trust; }
        set { _trust = AdjustToRange(value); }
    }

    public Relationship(string catFrom, string catTo, int romanticLove = 0, int platonicLike = 0, int dislike = 0,
        int admiration = 0, int comfortable = 0, int jealousy = 0, int trust = 0, List<string>? log = null)
    {
        log ??= new();

        this.catFrom = catFrom;
        this.catTo = catTo;
        this.romanticLove = romanticLove;
        this.platonicLike = platonicLike;
        this.dislike = dislike;
        this.admiration = admiration;
        this.comfortable = comfortable;
        this.trust = trust;
        this.jealousy = jealousy;
        this.log = log;
    }
    
    private int AdjustToRange(int value)
    {
        if (value > ValueMax)
        {
            value = ValueMax;
        }
        else if (value < ValueMin)
        {
            value = ValueMin;
        }

        return value;
    }

    // COMPLEX ADDS

    public void ComplexRomantic(int addValue, int buff)
    {
        buff = Math.Abs(buff);
        romanticLove += addValue;
        var sign = Math.Sign(addValue);
        platonicLike += sign * buff;
        comfortable += sign * buff;
        dislike += -sign * buff;
    }

    public void ComplexPlatonic(int addValue, int buff)
    {
        buff = Math.Abs(buff);
        platonicLike += addValue;
        var sign = Math.Sign(addValue);
        comfortable += sign * buff;
        dislike += -sign * buff;
    }

    public void ComplexDislike(int addValue, int buff)
    {
        buff = Math.Abs(buff);
        dislike += addValue;
        var sign = Math.Sign(addValue);
        platonicLike += -sign * buff;
        switch (sign)
        {
            case -1:
                romanticLove -= buff;
                break;
            case 1:
                comfortable += buff;
                break;
        }
    }

}
