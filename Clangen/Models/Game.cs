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

public class Game
{
    public World? currentWorld { get; set; }
    public GameConfig gameConfig { get; set; }
    public string test;

    public Game()
    {
        Console.WriteLine("creating game");
        test = "game!!";
    }

    /// <summary>
    /// Load a save, creating a Clan Object and putting it as
    /// the current clan. 
    /// </summary>
    public void LoadSave()
    {
        currentWorld = new World("New");
        currentWorld.PopulateClan();
    }

    /// <summary>
    /// Runs any tasks that need to be run at game startup.
    /// </summary>
    public void GameStart()
    {
        //Sprite.LoadResources();
        //TODO --> Needs to be run once at startup. 
        
        Sprites.LoadSprites();
        LoadSave();
        Console.WriteLine("Game-Start Tasks Complete");
    }

}
