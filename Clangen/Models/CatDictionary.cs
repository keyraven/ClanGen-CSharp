using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Clangen.Models.CatGroups;
using Clangen.Models.CatStuff;

namespace Clangen.Models;

public interface IReadOnlyFetchableObject<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
{
    TValue FetchCat(TKey catId);
}

public class CatDictionary : Dictionary<string, Cat>, IReadOnlyFetchableObject<string, Cat>
{
    [JsonIgnore]
    public string? fadedCatPath { get; set; } = null;

    [JsonIgnore] 
    public IFileSystem fileSystem { get; set; } = new FileSystem();
    
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
        if (fadedCatPath == null)
        {
            throw new Exception("Tried to load a faded cat, but there is no path set for faded cats. " +
                "Maybe the World hasn't been saved yet?");
        }

        if (!fileSystem.File.Exists(Path.Combine(fadedCatPath, catID)))
        {
            throw new Exception($"Unable to find faded cat with id: {catID}");
        }

        var options = new JsonSerializerOptions
        {
            IncludeFields = true,
            Converters = { new JsonStringEnumConverter() }
        };

        string jsonString = fileSystem.File.ReadAllText(Path.Combine(fadedCatPath, $"{catID}.json"));
        Cat? fadedCat = JsonSerializer.Deserialize<Cat>(jsonString, options);

        if (fadedCat is null)
        {
            throw new Exception($"Attempted to load faded cat with id {catID}, but cat is null.");
        }

        // Dummy faded group. 
        fadedCat.belongGroup = new Afterlife("0", this, "Faded");
        fadedCat.faded = true;

        return fadedCat;
    }
}