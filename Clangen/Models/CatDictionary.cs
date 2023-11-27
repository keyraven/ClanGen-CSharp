using System;
using System.Collections.Generic;
using Clangen.Models.CatStuff;

namespace Clangen.Models;

public interface IReadOnlyFetchableObject<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
{
    TValue FetchCat(TKey catId);
}

public class CatDictionary : Dictionary<string, Cat>, IReadOnlyFetchableObject<string, Cat>
{
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