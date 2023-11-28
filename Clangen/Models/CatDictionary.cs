using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Clangen.Models.CatStuff;

namespace Clangen.Models;

public interface IReadOnlyFetchableObject<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
{
    TValue FetchCat(TKey catId);
}

public class CatDictionary : Dictionary<string, Cat>, IReadOnlyFetchableObject<string, Cat>
{
    [JsonIgnore]
    public string fadedCatPath { get; set; } = "";
    
    public Cat FetchCat(string catId)
    {
        if (ContainsKey(catId))
        {
            return this[catId];
        }
        
        return LoadFadedCat(catId);
    }

    private Cat LoadFadedCat(string catID)
    {
        throw new NotImplementedException();
    }
}