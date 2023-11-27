using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using Clangen.Models.CatGroups;
using Clangen.Models.CatStuff;
using SkiaSharp;

namespace Clangen.Models;


/// <summary>
/// Holds information on a single save. 
/// </summary>
public partial class World
{
    // Relation Checking Function
    
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
    
    // Assigning Mentors. 

    public void CheckAndAdjustMentor(Cat check)
    {
        if (!check.status.IsApprentice())
        {
            check.RemoveMentor(this);
        }

        if (check.mentor == null || !check.IsValidMentor(FetchCat(check.mentor)))
        {
            AssignMentor(check);
        }
    }
    
    public Cat? AssignMentor(Cat apprentice)
    {
        List<Cat> validMentors = new();
        List<Cat> priorityMentors = new();
        foreach (var checkMentor in apprentice.belongGroup.GetMembers())
        {
            if (!apprentice.IsValidMentor(checkMentor)) { continue; }
            
            validMentors.Add(checkMentor);
            if (checkMentor.canWork && !checkMentor.apprentices.Any())
            {
                priorityMentors.Add(checkMentor);
            }
        }

        if (!validMentors.Any())
        {
            return null;
        }
        
        Cat chosenMentor = Utilities.ChoseRandom(priorityMentors.Any() ? priorityMentors : validMentors);
        apprentice.SetMentor(chosenMentor);
        return chosenMentor;
    }

    public void KillCat(Cat catToKill, Afterlife? targetAfterlife = null)
    {
        targetAfterlife ??= catToKill.belongGroup == currentClan ? starClan : unknownRes;
        
        CatsDeadOnCurrentTimeSkip.Add(catToKill.ID);
        catToKill.belongGroup = targetAfterlife;

    }
}

