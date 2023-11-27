using Clangen.Models;
using Clangen.Models.CatStuff;

namespace Clangen.Tests;

public class WorldTimeskipTest
{
    public World _testWorld = new("Test");

    public WorldTimeskipTest()
    {
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

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public void WhenATimeskipsArePreformed_ThenSumOfTimeskipsAndDeadForTimeSkipsShouldIncreaseByNumberOfSkips(int numberOfSkips)
    {
        PopulateWorld();
        
        // Gather current ages of cats by ID
        Dictionary<string, int> previousTimeskipsSum = new();
        foreach (var worldCat in _testWorld.GetAllCats())
        {
            previousTimeskipsSum.Add(worldCat.ID, worldCat.timeskips + worldCat.deadForTimeSkips);
        }
        
        _testWorld.PreformTimeSkips(numberOfSkips);

        bool aLeastOneCatChecked = false;
        foreach (var worldCat in _testWorld.GetAllCats())
        {
            if (!previousTimeskipsSum.ContainsKey(worldCat.ID))
            {
                continue;
            }

            aLeastOneCatChecked = true;
            int sumAfterTimeskips = worldCat.timeskips + worldCat.deadForTimeSkips;
            sumAfterTimeskips.Should().Be(previousTimeskipsSum[worldCat.ID] + numberOfSkips);
        }

        aLeastOneCatChecked.Should().BeTrue("because we need to check at least one cat");
    }
    
    [Fact]
    public void WhenAClanCatBecomesAnAdolescent_TheyShouldBecomeAnApprentice()
    {
        Cat testKitten = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, status: Cat.CatStatus.Kit,
            timeskips: Cat.AgeTimeskips[Cat.CatAge.Kitten][1]);
        _testWorld.AddCatToWorld(testKitten);
        
        _testWorld.PreformTimeSkips(1);

        testKitten.age.Should().Be(Cat.CatAge.Adolescent);
        testKitten.status.IsApprentice().Should().BeTrue("because clan kits should become apprentices when they" +
                                                         " become an adolescent");
    }

    [Fact]
    public void WhenAOutsiderBecomesAnAdolescent_TheyShouldNotBecomeAnApprentice()
    {
        Cat testKitten = new Cat(_testWorld.GetNextCatId(), _testWorld.outsiders, status: Cat.CatStatus.Kit,
            timeskips: Cat.AgeTimeskips[Cat.CatAge.Kitten][1]);
        _testWorld.AddCatToWorld(testKitten);
        
        _testWorld.PreformTimeSkips(1);

        testKitten.age.Should().Be(Cat.CatAge.Adolescent);
        testKitten.status.IsApprentice().Should().BeFalse("because outsider kits should become apprentices " +
                                                          "when they become an adolescent");
    }
    
}