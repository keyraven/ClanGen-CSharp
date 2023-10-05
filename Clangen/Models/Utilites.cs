using System;
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
    
    public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
    {
        if (collection == null)
        {
            throw new ArgumentNullException("Collection is null");
        }

        foreach (var item in collection)
        {
            if(!source.ContainsKey(item.Key)){ 
                source.Add(item.Key, item.Value);
            }
            else
            {
                // handle duplicate key issue here
            }  
        } 
    }
}