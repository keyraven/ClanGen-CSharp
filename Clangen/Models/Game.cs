using System;
using System.Collections.Generic;
using Clangen.Models.CatStuff;
using System.Text.Json;
using System.IO;


//Remove at some point, testing
using Clangen.Models.CatStuff;
using Clangen.Models.CatGroups;

namespace Clangen.Models;


public class GameConfig
{
    public Dictionary<Cat.CatAge, int[]> ageMoons { get; set; }

}

public class GameSettings
{
    public bool darkMode { get; set; } = false;
}

public static class Game
{
    public static readonly Random Rnd = new();
    public static World? currentWorld { get; set; }
    // TODO --> Fix the nullable warning that shows up here. 
    // TODO --> It also might be helpful to have a check to ensure this got 
    // TODO ---> everything that's needed. 
    public static GameConfig gameConfig = JsonSerializer.Deserialize<GameConfig>(
        File.ReadAllText("Resources/game_config.json"));

    /// <summary>
    /// Load a save, creating a Clan Object and putting it as
    /// the current clan. 
    /// </summary>
    public static void LoadSave()
    {
        currentWorld = new World("New");
        
        currentWorld.AddCatToWorld(Cat.GenerateRandomCat());
        
    }

    /// <summary>
    /// Runs any tasks that need to be run at game startup.
    /// </summary>
    public static void GameStart()
    {
        //Sprite.LoadResources();
        //TODO --> Needs to be run once at startup. 
        
        Sprites.LoadSprites();
        LoadSave();
        Console.WriteLine("Game-Start Tasks Complete");
    }

}
