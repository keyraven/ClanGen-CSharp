using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Clangen.Models.CatStuff;
using System.IO.Abstractions;

namespace Clangen.Models;


public class GameSettings
{
    public bool darkMode { get; set; } = false;
}

public class Game
{
    private readonly IFileSystem _fileSystem;
    
    //TODO: Add some proper logic to determine the save location. 
    public const string fadedCatFolderName = "fadedCats";
    public const string saveDirectory = "saves";
    public World? currentWorld { get; set; }

    public Game(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
        Console.WriteLine("creating game - passed filesystem. Testing?");
    }

    public Game() : this(fileSystem: new FileSystem())
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
        
        // First, load in the WorldSummary in order so we can check the hash. 
        WorldSummary worldSummary;
        try
        {
            string worldSummaryJsonString = _fileSystem.File.ReadAllText(Path.Combine(pathToWorldFolder, "worldSummary.json"));
            worldSummary = JsonSerializer.Deserialize<WorldSummary>(worldSummaryJsonString);
        }
        catch (JsonException)
        {
            return null; 
        }
        
        World loadedWorld;
        string jsonString;
        try
        {
            jsonString = _fileSystem.File.ReadAllText(Path.Combine(pathToWorldFolder, "world.json"));
            loadedWorld = JsonSerializer.Deserialize<World>(jsonString, options);
        }
        catch (JsonException)
        {
            return null; 
        }

        if (loadedWorld is null)
        {
            return null;
        }

        if (worldSummary.saveHash != 
            GetStringHash(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(jsonString))))
        {
            loadedWorld.saveFileEditingDetected = true;
            throw new WarningException("Save File Editing Detected!");
        }
        

        // Finish Up Tasks
        string worldFolder = new DirectoryInfo(pathToWorldFolder).Name;
        loadedWorld.ReplaceDeseralizedGroupIDWithGroupObjects();
        loadedWorld.SetFadedCatPath(Path.Combine(pathToWorldFolder, fadedCatFolderName));
        if (loadedWorld is not null) { loadedWorld.saveFolderName = worldFolder; }

        return loadedWorld;
    }

 
    public void SaveWorld(World world, string saveFolderPath)
    {
        if (!_fileSystem.Directory.Exists(saveFolderPath))
        {
            _fileSystem.Directory.CreateDirectory(saveFolderPath);
        }

        world.saveFolderName ??= GetAndCreateOpenWorldSaveFolder(saveFolderPath);

        var options = new JsonSerializerOptions
        {
            IncludeFields = true,
            Converters = { new JsonStringEnumConverter() }
        };

        
        // Fade Cats
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
            _fileSystem.File.WriteAllText(Path.Combine(saveFolderPath, world.saveFolderName, 
                fadedCatFolderName, $"{fadeCat.ID}.json"),fadedJsonString);
        }

        string jsonString = JsonSerializer.Serialize(world, options);
        
        // store a hashed version of the main save info, to store in the WorldSummary. Helps detect save-file
        // editing (if I must make them human readable)
        byte[] encoded = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(jsonString));
        Console.WriteLine(jsonString);
        Console.WriteLine(GetStringHash(encoded));
        
        _fileSystem.File.WriteAllText(Path.Combine(saveDirectory, world.saveFolderName, "world.json"),
            jsonString);
        
        jsonString = JsonSerializer.Serialize(world.GetWorldSummary(GetStringHash(encoded)), options);
        _fileSystem.File.WriteAllText(Path.Combine(saveDirectory, world.saveFolderName, "worldSummary.json"),
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
    public void GenerateRandomWorld()
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
        Console.WriteLine("Game-Start Tasks Complete");
    }

    private string GetAndCreateOpenWorldSaveFolder(string saveFolderPath)
    {
        int i = 0;
        while (true) 
        {
            if (++i > 10000)
            {
                throw new Exception("Unable to find unused name for World save folder");
            }

            string worldSaveFolderName = $"World{i}";
            if (_fileSystem.Directory.Exists(Path.Combine(saveFolderPath, worldSaveFolderName))) { continue; }

            SetUpWorldSaveFileLocation(saveFolderPath, worldSaveFolderName);
            return worldSaveFolderName;
        }
    }

    private void SetUpWorldSaveFileLocation(string saveFolderPath, string woldSaveFolderName)
    {
        _fileSystem.Directory.CreateDirectory(Path.Combine(saveFolderPath, woldSaveFolderName));
        _fileSystem.Directory.CreateDirectory(Path.Combine(saveFolderPath, woldSaveFolderName, fadedCatFolderName));
    }

    private string GetStringHash(byte[] hash)
    {
        return BitConverter.ToString(hash);
    }

}
