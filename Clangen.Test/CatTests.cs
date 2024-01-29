using Clangen.Models;
using Clangen.Models.CatStuff;
using Clangen.Models.CatGroups;

namespace Clangen.Tests;

/// <summary>
/// Testing anything with one one cat ---> NOT with relationships between cats.
/// No WORLD here! 
/// </summary>
public class OneCatTests
{
    private Group _dummyClan1 = new Clan("0", new CatDictionary(), "Dummy1");
    private int _lastId = 0;

    private string GetNextId() => _lastId++.ToString();
    
    [Fact]
    public void AgeTimeskipsShouldContainTwoValuesForEveryAge()
    {
        foreach (Cat.CatAge age in Enum.GetValues(typeof(Cat.CatAge)))
        {
            Cat.AgeTimeskips.Should().ContainKey(age).WhoseValue.Should().HaveCount(2);
        }
    }
    
    [Fact]
    public void WhenCatCreatedWithTimeSkipsInAgeRange_AgeShouldBeSetCorrectly()
    {
        foreach (Cat.CatAge age in Enum.GetValues(typeof(Cat.CatAge)))
        {
            int startingTimeskips = Cat.AgeTimeskips[age][0];
            Cat testCat = new Cat(GetNextId(), _dummyClan1, timeskips: startingTimeskips);

            testCat.age.Should().Be(age, $"the timeskips values was set in the range of {age}");
        }
    }
    
    [Fact]
    public void WhenAgeSetBeyondSeniorRange_ShouldStillBeSenior()
    {
        int startingTimeskips = Cat.AgeTimeskips[Cat.CatAge.Senior][1] + 1;
        Cat testCat = new Cat(GetNextId(), _dummyClan1, timeskips: startingTimeskips);

        testCat.age.Should().Be(Cat.CatAge.Senior, "because timeskips beyond the range for senior should still be senior");
    }
    
    [Theory]
    [InlineData(Cat.CatAge.Kitten, Cat.CatAge.Adolescent)]
    [InlineData(Cat.CatAge.Adolescent, Cat.CatAge.YoungAdult)]
    [InlineData(Cat.CatAge.YoungAdult, Cat.CatAge.Adult)]
    [InlineData(Cat.CatAge.Adult, Cat.CatAge.SeniorAdult)]
    public void WhenTimeskipsReachThreshold_ShouldGoToNext(Cat.CatAge startingAge, Cat.CatAge correctEndingAge)
    {
        // Setup
        int startingTimeskips = Cat.AgeTimeskips[startingAge][1];
        Cat testCat = new Cat(GetNextId(), _dummyClan1, timeskips: startingTimeskips);
        
        testCat.timeskips++;
        
        // Verify Age Changed
        testCat.age.Should().Be(correctEndingAge, $"timeskips were set to the last value for {startingAge}, then " +
                                               $"incremented, {correctEndingAge} comes next");
    }

    [Theory]
    // Test both even and odd. 
    [InlineData(11)]
    [InlineData(12)]
    public void MoonsShouldBeAFourthOfTimeskips(int currentTimeskips)
    {
        Cat testCat = new Cat(GetNextId(), _dummyClan1, timeskips: currentTimeskips);
        testCat.moons.Should().Be(((float)currentTimeskips) / 4);
    }
}