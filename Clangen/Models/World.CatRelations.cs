using System;
using System.Collections.Generic;
using System.Linq;
using Clangen.Models.CatGroups;
using Clangen.Models.CatStuff;

namespace Clangen.Models;


/// <summary>
/// Holds information on a single save. 
/// </summary>
public partial class World
{
    // Relation Checking Function

    public bool CheckRelated(Cat cat1, Cat cat2)
    {
        return cat1 == cat2 || CheckParent(cat1, cat2) || CheckParent(cat2, cat1) || 
               CheckGrandparent(cat1, cat2) || CheckGrandparent(cat2, cat1) || 
               CheckSiblings(cat1, cat2) || CheckParentsSibling(cat1, cat2) ||
               CheckParentsSibling(cat2, cat1) || CheckFirstCousin(cat1, cat2);
    }
    
    public bool CheckParent(Cat parent, Cat child)
    {
        return child.bioParents.Contains(parent.ID) || child.adoptiveParents.Contains(parent.ID);
    }
    
    public bool CheckGrandparent(Cat grandparent, Cat grandchild)
    {
        foreach (var parentId in grandchild.bioParents.Concat(grandchild.adoptiveParents))
        {
            Cat parent = FetchCat(parentId);
            if (CheckParent(grandparent, parent))
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckSiblings(Cat sibling1, Cat sibling2)
    {
        if (sibling1 == sibling2) { return false; }
        
        IEnumerable<string> allParents = sibling1.bioParents.Concat(sibling1.adoptiveParents);
        return allParents.Intersect(sibling2.bioParents.Concat(sibling2.adoptiveParents)).Any();
    }

    public bool CheckParentsSibling(Cat parentsSibling, Cat siblingsChild)
    {
        foreach (var parentId in siblingsChild.bioParents.Concat(siblingsChild.adoptiveParents))
        {
            Cat parent = FetchCat(parentId);
            if (CheckSiblings(parent, parentsSibling))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckFirstCousin(Cat cousin1, Cat cousin2)
    {
        if (cousin1 == cousin2) { return false; }
        
        // If cats are siblings, this should return false
        if (CheckSiblings(cousin1, cousin2))  { return false; }
        
        List<string> grandparents1 = new();
        foreach (var parentId in cousin1.bioParents.Concat(cousin1.adoptiveParents))
        {
            Cat parent = FetchCat(parentId);
            grandparents1.AddRange(parent.bioParents);
            grandparents1.AddRange(parent.adoptiveParents);
        }

        List<string> grandparents2 = new();
        foreach (var parentId in cousin2.bioParents.Concat(cousin2.adoptiveParents))
        {
            Cat parent = FetchCat(parentId);
            grandparents2.AddRange(parent.bioParents);
            grandparents2.AddRange(parent.adoptiveParents);
        }

        return grandparents1.Intersect(grandparents2).Any();
    }

    public bool IsPotentialMate(Cat cat1, Cat cat2, bool applyAgeGapRestriction = false, bool notAdultAlwaysFalse = true)
    {
        if (CheckRelated(cat1, cat2)) { return false; }

        if (cat1.dead != cat2.dead) { return false; }
        
        if (!cat1.age.IsAdult() || !cat1.age.IsAdult())
        {
            if (notAdultAlwaysFalse) { return false; }

            if (cat1.age != cat2.age) { return false; }
        }
        else if (applyAgeGapRestriction)
        {
            if (cat1.age != cat2.age && Math.Abs(cat1.timeskips - cat2.timeskips) > 80)
            {
                return false;
            }
        }

        if (cat1.ID == cat2.mentor || cat2.ID == cat1.mentor) { return false; }
        
        return true;
    }
    
    // Assigning Mentors. 

    public void CheckAndAdjustMentor(Cat check)
    {
        if (!check.status.IsApprentice())
        {
            check.RemoveMentor();
        }

        if (check.mentor == null || !check.IsValidMentor(FetchCat(check.mentor)))
        {
            AssignMentor(check);
        }
    }
    
    public Cat? AssignMentor(Cat apprentice)
    {
        List<Cat> validMentors = [];
        List<Cat> priorityMentors = [];
        foreach (var checkMentor in apprentice.belongGroup.GetMembers())
        {
            if (!apprentice.IsValidMentor(checkMentor)) { continue; }
            
            validMentors.Add(checkMentor);
            if (checkMentor.canWork && checkMentor.apprentices.Count == 0)
            {
                priorityMentors.Add(checkMentor);
            }
        }

        if (validMentors.Count == 0)
        {
            return null;
        }
        
        Cat chosenMentor = Utilities.ChoseRandom(priorityMentors.Count == 0 ? validMentors : priorityMentors );
        apprentice.SetMentor(chosenMentor);
        return chosenMentor;
    }

    public void KillCat(Cat catToKill, Afterlife? targetAfterlife = null)
    {
        targetAfterlife ??= catToKill.belongGroup == currentClan ? starClan : unknownRes;
        
        catsDeadOnCurrentTimeSkip.Add(catToKill.ID);
        catToKill.belongGroup = targetAfterlife;

    }
}

