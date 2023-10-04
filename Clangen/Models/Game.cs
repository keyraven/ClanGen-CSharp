using System;
using System.Collections.Generic;
using Clangen.Models.CatStuff;
using System.Text.Json;
using System.IO;

namespace Clangen.Models;


public class GameConfig
{
    public Dictionary<Cat.CatAge, int[]> ageMoons { get; set; }

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
        currentWorld = new World();
    }

    /// <summary>
    /// Runs any tasks that need to be run at game startup.
    /// </summary>
    public static void GameStart()
    {
        //Sprite.LoadResources();
        //TODO --> Needs to be run once at startup. 
        LoadSave();
        Console.WriteLine("Game-Start Tasks Complete");
    }

}
