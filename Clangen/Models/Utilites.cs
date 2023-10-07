using System;
using System.Collections.Generic;
using System.Linq;

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
    
    /// <summary>
    /// Add multiple dictionaries together. If there are duplicate keys,
    /// this uses the value from the new dictionary. 
    /// </summary>
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
                source[item.Key] = item.Value;
            }  
        } 
    }
    
    /// <summary>
    /// Break a list of items into chunks of a specific size
    /// </summary>
    public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
    {
        while (source.Any())
        {
            yield return source.Take(chunksize);
            source = source.Skip(chunksize);
        }
    }
}