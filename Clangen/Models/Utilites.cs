using System.Collections.Generic;
namespace Clangen.Models;

public static class Utilities
{
    /// <summary>
    /// Choose a random entry from a list. 
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>A random entry from the passed list.</returns>
    public static T ChoseRandom<T>(List<T> list)
    {
        return list[Game.Rnd.Next(0, list.Count)];
    }
}