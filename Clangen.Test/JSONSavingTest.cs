using System.Text.Json;
using System.Text.Json.Serialization;
using Clangen.Models;
using Clangen.Models.CatStuff;
using Xunit.Abstractions;

namespace Clangen.Tests;

public class JSONSavingTest
{
    private Game _game { get; set; } = new Game();

    private readonly ITestOutputHelper output;

    public JSONSavingTest(ITestOutputHelper output)
    {
        this.output = output;

        _game.GameStart();
        _game.currentWorld = new World("Test");
        _game.currentWorld.saveFolderName = "World1";
    }
    
    [Fact]
    public void TestJson()
    {
        PopulateWorld();

        _game.SaveWorld();
        
    }
    
    private void PopulateWorld()
    {   
        if (_game.currentWorld == null) { return; }

        // All some clan cats
        for (int i = 0; i < 6; i++)
        {
            Cat warrior = new Cat(_game.currentWorld.GetNextCatId(), _game.currentWorld.currentClan, Cat.CatStatus.Warrior);
            _game.currentWorld.AddCatToWorld(warrior);
        }

        Cat outsider = new Cat(_game.currentWorld.GetNextCatId(), _game.currentWorld.outsiders, Cat.CatStatus.Rogue);
        _game.currentWorld.AddCatToWorld(outsider);

    }
}