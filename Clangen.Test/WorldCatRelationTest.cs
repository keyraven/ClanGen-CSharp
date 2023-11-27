using Clangen.Models.CatStuff;
using Clangen.Models;

namespace Clangen.Tests;

public class WorldCatRelationTest
{

    private World _testWorld = new World("test");
    
    [Fact]
    public void ParentCheckingShouldBeTrue_WhenOneCatIsBiologicalParentOfOther()
    {
        Cat bioParent = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat bioChild = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: new() { bioParent.ID });
        _testWorld.AddCatToWorld(bioParent);
        _testWorld.AddCatToWorld(bioChild);

        _testWorld.CheckParent(bioParent, bioChild).Should().BeTrue();
        _testWorld.CheckParent(bioChild, bioParent).Should().BeFalse("because it's the other way around");
        _testWorld.CheckRelated(bioChild, bioParent).Should().BeTrue();
        _testWorld.CheckRelated(bioParent, bioChild).Should().BeTrue();
    }
    
    [Fact]
    public void ParentCheckingShouldBeTrue_WhenOneCatIsAdoptiveParentOfOther()
    {
        Cat adoptiveParent = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat bioChild = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, adoptiveParents: new() { adoptiveParent.ID });
        _testWorld.AddCatToWorld(adoptiveParent);
        _testWorld.AddCatToWorld(bioChild);

        _testWorld.CheckParent(adoptiveParent, bioChild).Should().BeTrue();
        _testWorld.CheckParent(bioChild, adoptiveParent).Should().BeFalse("because it's the other way around");
        _testWorld.CheckRelated(bioChild, adoptiveParent).Should().BeTrue();
        _testWorld.CheckRelated(adoptiveParent, bioChild).Should().BeTrue();
    }

    [Fact]
    public void CheckRelatedShouldBeFalseWhenCatsAreNotRelated()
    {
        Cat cat1 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat cat2 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        _testWorld.AddCatToWorld(cat1);
        _testWorld.AddCatToWorld(cat2);
        
        _testWorld.CheckRelated(cat1, cat2).Should().BeFalse();
        _testWorld.CheckRelated(cat2, cat1).Should().BeFalse();
    }

    [Fact]
    public void SiblingCheckingShouldBeTrue_WhenCatsAreSiblingsThroughBiologicalParent()

    {
        Cat commonParent = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat sibling1 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: new() {commonParent.ID});
        Cat sibling2 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: new() {commonParent.ID});
        _testWorld.AddCatToWorld(commonParent);
        _testWorld.AddCatToWorld(sibling1);
        _testWorld.AddCatToWorld(sibling2);

        _testWorld.CheckSiblings(sibling1, sibling2).Should().BeTrue("because these cats share a biological parent");
        _testWorld.CheckSiblings(sibling2, sibling1).Should().BeTrue("because these cats share a biological parent");
        _testWorld.CheckRelated(sibling1, sibling2).Should().BeTrue("because they are biological siblings");
        _testWorld.CheckRelated(sibling2, sibling1).Should().BeTrue("because they are biological siblings");
    }
    
    [Fact]
    public void SiblingCheckingShouldBeTrue_WhenCatsAreSiblingsThroughAdoptiveParent()

    {
        Cat commonParent = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat sibling1 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, adoptiveParents: new() {commonParent.ID});
        Cat sibling2 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, adoptiveParents: new() {commonParent.ID});
        _testWorld.AddCatToWorld(commonParent);
        _testWorld.AddCatToWorld(sibling1);
        _testWorld.AddCatToWorld(sibling2);

        _testWorld.CheckSiblings(sibling1, sibling2).Should().BeTrue("because these cats share an adoptive parent");
        _testWorld.CheckSiblings(sibling2, sibling1).Should().BeTrue("because these cats share an adoptive parent");
        _testWorld.CheckRelated(sibling1, sibling2).Should().BeTrue("because they are adoptive siblings");
        _testWorld.CheckRelated(sibling2, sibling1).Should().BeTrue("because they are adoptive siblings");
    }
    
    [Fact]
    public void SiblingCheckingShouldBeTrue_WhenCatsAreSiblingsThroughMixedParent()

    {
        Cat commonParent = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat sibling1 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: new() {commonParent.ID});
        Cat sibling2 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, adoptiveParents: new() {commonParent.ID});
        _testWorld.AddCatToWorld(commonParent);
        _testWorld.AddCatToWorld(sibling1);
        _testWorld.AddCatToWorld(sibling2);

        _testWorld.CheckSiblings(sibling1, sibling2).Should().BeTrue("because these cats share an biological/adoptive parent");
        _testWorld.CheckSiblings(sibling2, sibling1).Should().BeTrue("because these cats share an biological/adoptive parent");
        _testWorld.CheckRelated(sibling1, sibling2).Should().BeTrue("because they are siblings");
        _testWorld.CheckRelated(sibling2, sibling1).Should().BeTrue("because they are siblings");
    }

    [Fact]
    public void GrandParentCheckIsTrue_WhenCatIsBiologicalGrandparentOfOther()
    {
        Cat grandparent = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat parent = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: new() { grandparent.ID});
        Cat grandchild = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: new() { parent.ID});
        _testWorld.AddCatToWorld(grandparent);
        _testWorld.AddCatToWorld(parent);
        _testWorld.AddCatToWorld(grandchild);

        _testWorld.CheckGrandparent(grandparent, grandchild).Should().BeTrue();
        _testWorld.CheckGrandparent(grandchild, grandparent).Should().BeFalse("because it's the other way around.");
        _testWorld.CheckRelated(grandparent, grandchild).Should().BeTrue("because they are grandparent/grandchild");
        _testWorld.CheckRelated(grandchild, grandparent).Should().BeTrue("because they are grandparent/grandchild");
    }
}