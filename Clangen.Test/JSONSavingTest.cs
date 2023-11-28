using System.Text.Json;
using System.Text.Json.Serialization;
using Clangen.Models;
using Clangen.Models.CatStuff;
using Xunit.Abstractions;

namespace Clangen.Tests;

public class JSONSavingTest
{
    private World _testWorld { get; set; }

    private readonly ITestOutputHelper output;

    public JSONSavingTest(ITestOutputHelper output)
    {
        this.output = output;
        _testWorld = new World("Test");
    }
    
    [Fact]
    public void TestJson()
    {
        PopulateWorld();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            IncludeFields = true,
            Converters = { new JsonStringEnumConverter() }
        };
        
        string json = JsonSerializer.Serialize(_testWorld, options);
        output.WriteLine(json);
        World? recoveredWorld = JsonSerializer.Deserialize<World>(json, options);

        json = JsonSerializer.Serialize(recoveredWorld, options);
        output.WriteLine(json);
    }
    
    private void PopulateWorld()
    {
        // All some clan cats
        for (int i = 0; i < 6; i++)
        {
            Cat warrior = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, Cat.CatStatus.Warrior);
            _testWorld.AddCatToWorld(warrior);
        }

        Cat outsider = new Cat(_testWorld.GetNextCatId(), _testWorld.outsiders, Cat.CatStatus.Rogue);
        _testWorld.AddCatToWorld(outsider);

    }
}