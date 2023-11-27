using System;
using System.Collections.Generic;
using System.Linq;

namespace Clangen.Models;

public static class Utilities
{
    private static readonly Random Rnd = new();
    
    /// <summary>
    /// Choose a random entry from a list. 
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>A random entry from the passed list.</returns>
    public static T ChoseRandom<T>(List<T> list)
    {
        return list[Rnd.Next(0, list.Count)];
    }
    
    /// <summary>
    /// Choose a random entry from a list. 
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>A random entry from the passed list.</returns>
    public static T ChoseRandom<T>(IReadOnlyList<T> list)
    {
        return list[Rnd.Next(0, list.Count)];
    }

    /// <summary>
    /// Makes a roll where the chance of success (true return) is 1/inverseChance
    /// If inverseChance == 0, chance is 1. 
    /// </summary>
    /// <param name="inverseChance"> Inverse Chance of true return </param>
    /// <returns></returns>
    public static bool InverseChanceRoll(int inverseChance)
    {
        return Rnd.Next(0, inverseChance) == 0;
    }

    /// <summary>
    /// Makes a roll where the percentage chance of success if chanceOfSuccess
    /// </summary>
    /// <param name="chanceOfSuccess"> percentage chance of success. </param>
    /// <returns></returns>
    public static bool PercentageRoll(int chanceOfSuccess)
    {
        return Rnd.Next(1, 100) <= chanceOfSuccess;
    }
    
    /// <summary>
    /// Generate a random integer. 
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int RandomInt(int min, int max)
    {
        return Rnd.Next(min, max);
    }

    public static int RandomInt(int[] range)
    {
        if (range.Length < 2)
        {
            throw new ArgumentException("RandomInt: range must have a length of at least 2.");
        }

        return Rnd.Next(range[0], range[1]);
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
}