using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Clangen.Models.CatStuff;

namespace Clangen.Models;


public class GameSettings
{
    public bool darkMode { get; set; } = false;
}

public class Game
{
    //TODO: Add some proper logic to determine the save location. 
    public const string fadedCatFolderName = "fadedCats";
    public const string saveDirectory = "saves";
    public World? currentWorld { get; set; }

    public Game()
    {
        Console.WriteLine("creating game");
    }

    public World? LoadWorld(string pathToWorldFolder)
    {
        var options = new JsonSerializerOptions
        {
            IncludeFields = true,
            Converters = { new JsonStringEnumConverter() }
        };

        string worldFolder = new DirectoryInfo(pathToWorldFolder).Name;

        string jsonString = File.ReadAllText(Path.Combine(pathToWorldFolder, "world.json"));
        World? loadedWorld = null;
        try { loadedWorld = JsonSerializer.Deserialize<World?>(jsonString, options); }
        catch (JsonException) { return null; }
        

        // Finish Up Tasks
        loadedWorld?.ReplaceDeseralizedGroupIDWithGroupObjects();
        loadedWorld?.SetFadedCatPath(Path.Combine(pathToWorldFolder, fadedCatFolderName));
        if (loadedWorld is not null) { loadedWorld.saveFolderName = worldFolder; }

        return loadedWorld;
    }

 
    public void SaveWorld(World world, string saveFolderPath)
    {
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }

        world.saveFolderName ??= GetAndCreateOpenWorldSaveFolder(saveFolderPath);

        var options = new JsonSerializerOptions
        {
            IncludeFields = true,
            Converters = { new JsonStringEnumConverter() }
        };

        
        // Fade Cats, Offically
        foreach (var fadeCat in world.GetCatsToFade())
        {
            //Remove mentor, if needed
            fadeCat.RemoveMentor();

            //Remove mates, if needed
            foreach (var mateID in fadeCat.mates)
            {
                fadeCat.RemoveMate(mateID);
            }

            string fadedJsonString = JsonSerializer.Serialize(fadeCat, options);
            File.WriteAllText(Path.Combine(saveFolderPath, world.saveFolderName, 
                fadedCatFolderName, $"{fadeCat.ID}.json"),fadedJsonString);
        }

        string jsonString = JsonSerializer.Serialize(world, options);
        File.WriteAllText(Path.Combine(saveDirectory, world.saveFolderName, "world.json"),
            jsonString);
        jsonString = JsonSerializer.Serialize(world.GetWorldSummary(), options);
        File.WriteAllText(Path.Combine(saveDirectory, world.saveFolderName, "worldSummary.json"),
            jsonString);
    }

    /// <summary>
    /// Gets all saved worlds in the save directory/ 
    /// </summary>
    /// <returns>A dictionary, keyed with the name of the save folder, </returns>
    public Dictionary<string, WorldSummary> GetAllSavedWorlds(string saveFolderName)
    {
        Dictionary<string, WorldSummary> output = new();
        foreach (var worldFolderPath in Directory.GetDirectories(saveFolderName))
        {
            if (File.Exists(Path.Combine(worldFolderPath, "world.json")) && 
                File.Exists(Path.Combine(worldFolderPath, "worldSummary.json")))
            {
                string jsonstring = File.ReadAllText(Path.Combine(worldFolderPath, "worldSummary.json"));
                WorldSummary worldSummary;
                try
                {
                    worldSummary = JsonSerializer.Deserialize<WorldSummary>(jsonstring);
                }
                catch (JsonException) { continue; }
                
                output.Add(new DirectoryInfo(worldFolderPath).Name, worldSummary);
            }
        }

        return output;
    }

    /// <summary>
    /// Load a save, creating a Clan Object and putting it as
    /// the current clan. 
    /// </summary>
    public void LoadWorld()
    {
        currentWorld = new World("New");
        currentWorld.PopulateClan();
    }

    /// <summary>
    /// Runs any tasks that need to be run at game startup.
    /// </summary>
    public void GameStart()
    {
        
        Sprites.LoadSprites();
        LoadWorld();
        Console.WriteLine("Game-Start Tasks Complete");
    }

    public string GetAndCreateOpenWorldSaveFolder(string saveFolderPath)
    {
        int i = 0;
        while (true) 
        {
            if (++i > 10000)
            {
                throw new Exception("Unable to find unused name for World save folder");
            }

            string worldSaveFolderName = $"World{i}";
            if (Directory.Exists(Path.Combine(saveFolderPath, worldSaveFolderName))) { continue; }

            Directory.CreateDirectory(Path.Combine(worldSaveFolderName, worldSaveFolderName));
            return worldSaveFolderName;
        }
    }



}
