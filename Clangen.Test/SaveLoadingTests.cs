using System.IO.Abstractions.TestingHelpers;
using Clangen.Models;
using Xunit.Abstractions;

namespace Clangen.Tests;

// ReWrite these to be NOT bad. 
public class SaveLoadingTests
{
    private readonly MockFileSystem _filesystem;
    private readonly Game _game;
    private readonly ITestOutputHelper _output;
    
    public SaveLoadingTests(ITestOutputHelper output)
    {
        _output = output;
        _filesystem = new MockFileSystem();
        _game = new Game(_filesystem);
    }
    
    [Fact]
    public void WorldSavedToCorrectLocation()
    {
        _game.GameStart();
        _game.GenerateRandomWorld();
        
        // Set Up File System
        var saveFolderPath = Path.Combine("saves");
        _filesystem.AddDirectory("saves");
        
        _game.SaveWorld(_game.currentWorld, saveFolderPath);

        var worldFileContents = _filesystem.GetFile(Path.Combine("saves", "World1", "world.json"));
        var worldSummaryFileContents = _filesystem.GetFile(Path.Combine("saves", "World1", "worldSummary.json"));

        worldFileContents.TextContents.Should().NotBeEmpty();
        worldSummaryFileContents.TextContents.Should().NotBeEmpty();
    }
    
    [Fact]
    public void WorldCanBeRecoveredFromJSONdata()
    {
        _game.GameStart();
        _game.GenerateRandomWorld();
        
        // Set Up File System
        var saveFolderPath = Path.Combine("saves", "World1");
        _filesystem.AddDirectory("saves");
        _filesystem.AddDirectory(saveFolderPath);
        _game.currentWorld.saveFolderName = "World1";
        
        _game.SaveWorld(_game.currentWorld, saveFolderPath);

        World newWorld = _game.LoadWorld(saveFolderPath);
        Console.WriteLine(newWorld.ToString());
        
        _output.WriteLine(_filesystem.GetFile(Path.Combine("saves", "World1", "world.json")).TextContents);
        newWorld.saveFolderName = "World2";
        _game.SaveWorld(newWorld, Path.Combine("saves", "World2"));
        
        _output.WriteLine(_filesystem.GetFile(Path.Combine("saves", "World2", "world.json")).TextContents);
        (_filesystem.GetFile(Path.Combine("saves", "World2", "world.json")).TextContents == 
         _filesystem.GetFile(Path.Combine("saves", "World1", "world.json")).TextContents)
            .Should().BeTrue();
    }
}