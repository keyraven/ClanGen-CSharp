using Clangen.Models.CatStuff;

namespace Clangen.Models;

// This is for various timeskip actions

public partial class World
{
    
    /// <summary>
    /// Preform TimeSkips. Allows multiple timeskips to be done back-to-back.
    /// Each TimeSkip in half a moon. 
    /// </summary>
    /// <param name="number"></param>
    public void PreformTimeSkips(int number = 1)
    {
        // Clear Event Dictionaries
        mediated.Clear();
        currentEvents.Clear();
        medicineDenEvents.Clear();
        
        for (int i = 0; i < number; i++)
        {
            TimeSkip();
        }
        
        //TODO - Set cat thoughts, 
    }
    
    /// <summary>
    /// Preforms a timeskip
    /// </summary>
    private void TimeSkip()
    {
        timeskips += 1;
        foreach (var cat in _allCats)
        {
            SingleCatTimeSkip(cat.Value);
        }
    }

    private void SingleCatTimeSkip(Cat cat)
    {
        cat.TimeSkip();
    }
    
}