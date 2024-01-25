using Clangen.Models.CatStuff;
using Clangen.Models;

namespace Clangen.Tests;

public class WorldCatRelationTest
{

    private World _testWorld = new World("test");

    [Fact]
    public void CatShouldBeTreatedAsRelatedToThemselves()
    {
        Cat singleCat = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        _testWorld.AddCatToWorld(singleCat);

        _testWorld.CheckRelated(singleCat, singleCat).Should().BeTrue();
    }

    [Fact]
    public void SiblingCheckingShouldBeFalse_WhenCheckingACatAgainstThemselves()
    {
        // If a cat has a parent(s), you could get a case where a cat is treated as their own sibling. 
        
        Cat bioParent = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat bioChild = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: [ bioParent.ID ]);
        _testWorld.AddCatToWorld(bioParent);
        _testWorld.AddCatToWorld(bioChild);

        _testWorld.CheckSiblings(bioChild, bioChild).Should().BeFalse();
    }
    
    [Fact]
    public void CousinCheckingShouldBeFalse_WhenCheckingACatAgainstThemselves()
    {
        // If a cat has grandparents(s), you could get a case where a cat is treated as their own cousin. 
        
        Cat bioGrandparent = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat bioParent = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: [ bioGrandparent.ID ]);
        Cat bioChild = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: [ bioParent.ID ]);
        _testWorld.AddCatToWorld(bioGrandparent);
        _testWorld.AddCatToWorld(bioParent);
        _testWorld.AddCatToWorld(bioChild);

        _testWorld.CheckFirstCousin(bioChild, bioChild).Should().BeFalse();
    }
    
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
    public void CheckRelatedShouldBeFalse_WhenCatsAreNotRelatedAndHaveNoParents()
    {
        Cat cat1 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat cat2 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        _testWorld.AddCatToWorld(cat1);
        _testWorld.AddCatToWorld(cat2);
        
        _testWorld.CheckRelated(cat1, cat2).Should().BeFalse();
        _testWorld.CheckRelated(cat2, cat1).Should().BeFalse();
    }
    
    [Fact]
    public void CheckRelatedShouldBeFalse_WhenCatsAreNotRelatedAndHaveAParent()
    {
        Cat parent1 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat parent2 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat cat1 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: [parent1.ID]);
        Cat cat2 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: [parent2.ID]);
        _testWorld.AddCatToWorld(parent1);
        _testWorld.AddCatToWorld(parent2);
        _testWorld.AddCatToWorld(cat1);
        _testWorld.AddCatToWorld(cat2);
        
        _testWorld.CheckRelated(cat1, cat2).Should().BeFalse();
        _testWorld.CheckRelated(cat2, cat1).Should().BeFalse();
    }
    
    [Fact]
    public void SiblingCheckingShouldBeTrue_WhenCatsAreSiblingsThroughBiologicalParent()

    {
        Cat commonParent = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat sibling1 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: [ commonParent.ID ] );
        Cat sibling2 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: [ commonParent.ID ]);
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
    public void GrandParentCheckShouldBeTrue_WhenCatIsBiologicalGrandparentOfOther()
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

    [Fact]
    public void CousinCheckingShouldBeFalse_WhenCatsAreSiblings()
    {
        // Siblings will also share grandparents, so you could get a false positive if you aren't careful. 
        
        Cat bioGrandparent = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat commonParent = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: [ bioGrandparent.ID ]);
        Cat sibling1 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: [ commonParent.ID ] );
        Cat sibling2 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: [ commonParent.ID ]);
        _testWorld.AddCatToWorld(bioGrandparent);
        _testWorld.AddCatToWorld(commonParent);
        _testWorld.AddCatToWorld(sibling1);
        _testWorld.AddCatToWorld(sibling2);

        _testWorld.CheckFirstCousin(sibling1, sibling2).Should().BeFalse("Since they are siblings, not cousins");
        // Check the inverse to be sure. 
        _testWorld.CheckFirstCousin(sibling2, sibling1).Should().BeFalse("Since they are siblings, not cousins");
    }
    
    
    [Fact]
    public void CousinCheckShouldBeTrue_WhenCatsShareBiologicalGrandparent()
    {
        Cat bioGrandparent = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat bioParent1= new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: [ bioGrandparent.ID ]);
        Cat bioParent2 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: [ bioGrandparent.ID ]);
        Cat bioCousin1 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: [ bioParent1.ID ]);
        Cat bioCousin2 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, bioParents: [ bioParent2.ID ]);
        _testWorld.AddCatToWorld(bioGrandparent);
        _testWorld.AddCatToWorld(bioParent1);
        _testWorld.AddCatToWorld(bioParent2);
        _testWorld.AddCatToWorld(bioCousin1);
        _testWorld.AddCatToWorld(bioCousin2);

        _testWorld.CheckFirstCousin(bioCousin1, bioCousin2).Should().BeTrue("because they share a biological grandparent");
        _testWorld.CheckFirstCousin(bioCousin2, bioCousin1).Should().BeTrue("because they share a biological grandparent");
    }
    
    [Fact]
    public void CousinCheckShouldBeTrue_WhenCatsShareAdoptiveGrandparent()
    {
        Cat adoptiveGrandparent = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan);
        Cat adoptiveParent1= new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, adoptiveParents: [ adoptiveGrandparent.ID ]);
        Cat adoptiveParent2 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, adoptiveParents: [ adoptiveGrandparent.ID ]);
        Cat adoptiveCousin1 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, adoptiveParents: [ adoptiveParent1.ID ]);
        Cat adoptiveCousin2 = new Cat(_testWorld.GetNextCatId(), _testWorld.currentClan, adoptiveParents: [ adoptiveParent2.ID ]);
        _testWorld.AddCatToWorld(adoptiveGrandparent);
        _testWorld.AddCatToWorld(adoptiveParent1);
        _testWorld.AddCatToWorld(adoptiveParent2);
        _testWorld.AddCatToWorld(adoptiveCousin1);
        _testWorld.AddCatToWorld(adoptiveCousin2);

        _testWorld.CheckFirstCousin(adoptiveCousin1, adoptiveCousin2).Should().BeTrue("because they share a biological grandparent");
        _testWorld.CheckFirstCousin(adoptiveCousin2, adoptiveCousin1).Should().BeTrue("because they share a biological grandparent");
    }
}