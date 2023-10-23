﻿using Clangen.Models.CatStuff;

namespace Clangen.Models;

// This is for various timeskip actions

public partial class World
{
    public void PreformTimeSkips(int number = 1)
    {
        // Clear Event Dictionaries
        mediated.Clear();
        currentEvents.Clear();
        medicineDenEvents.Clear();
        patrolled.Clear();
        
        for (int i = 0; i < number; i++)
        {
            TimeSkip();
        }
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