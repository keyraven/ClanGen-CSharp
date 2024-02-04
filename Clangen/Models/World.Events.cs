using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Clangen.Models.CatStuff;
using Clangen.Models.Events;
using Clangen.Models.CatGroups;

namespace Clangen.Models;

// This is for various timeskip actions
// Note- maybe move to it's own class. 

public partial class World
{

    [JsonIgnore]
    public List<string> catsDeadOnCurrentTimeSkip { get; set; } = new();
    
    /// <summary>
    /// Preform TimeSkips. Allows multiple timeskips to be done back-to-back.
    /// Each TimeSkip in half a moon. 
    /// </summary>
    /// <param name="number"></param>
    public void PreformTimeSkips(int number = 1)
    {
        // Clear Event Dictionaries
        mediated.Clear();
        currentEvents.Clear();
        medicineDenEvents.Clear();
        
        for (int i = 0; i < number; i++)
        {
            TimeSkip();
        }
        
        //TODO - Set cat thoughts, 
    }
    
    /// <summary>
    /// Preforms a timeskip
    /// </summary>
    private void TimeSkip()
    {
        catsDeadOnCurrentTimeSkip.Clear();
        timeskips += 1;
        
        
        foreach (var entry in _allCats)
        {
            AllCatsTimeskip(entry.Value);
            
            if (entry.Value.belongGroup == currentClan)
            {
                AliveClanCatTimeskip(entry.Value);
            }
            else if (entry.Value.dead)
            {
                DeadCatTimeSkip(entry.Value);
            }
            else if (entry.Value.belongGroup == outsiders)
            {
                OutsideCatTimeSkip(entry.Value);
            }
            else
            {
                OtherCatTimeSkip(entry.Value);
            }
        }
        
    }
    
    /// <summary>
    /// Tasks that need to be done for all cats, regardless of group/dead status. 
    /// </summary>
    /// <param name="currentCat"></param>
    private void AllCatsTimeskip(Cat currentCat)
    {
        if (!currentCat.dead || catsDeadOnCurrentTimeSkip.Contains(currentCat.ID))
        {
            currentCat.timeskips++;
        }
        else
        {
            currentCat.deadForTimeSkips++;
        }
    }
    
    private void AliveClanCatTimeskip(Cat currentCat)
    {
        // Check mentor is still valid, reassign if not. 
        CheckAndAdjustMentor(currentCat);
        
        // Update and Check Status, Preform Ceremonies
        CheckAndUpdateStatus(currentCat);
        
    }

    private void DeadCatTimeSkip(Cat currentCat)
    {
        
    }

    private void OutsideCatTimeSkip(Cat currentCat)
    {
        
    }

    private void OtherCatTimeSkip(Cat currentCat)
    {
        
    }
    
    private void CheckAndUpdateStatus(Cat currentCat)
    {
        // Updating Status Kit --> Apprentice
        if (currentCat.timeskips == Cat.AgeTimeskips[Cat.CatAge.Adolescent][0])
        {
            MakeApprentice(currentCat);
        }
        // Updating Status Apprentice --> Full Status
        else if (currentCat.status.IsApprentice() && currentCat.timeskips == Cat.AgeTimeskips[Cat.CatAge.YoungAdult][0])
        {
            GraduateFromApprenticeship(currentCat);
        }
        else if (currentCat.status.CanRetire() && currentCat.timeskips == Cat.AgeTimeskips[Cat.CatAge.Senior][0])
        {
            Retire(currentCat);
        }
    }

    private void MakeApprentice(Cat currentCat)
    {
        var apprenticeType = Utilities.InverseChanceRoll(100)
            ? Cat.CatStatus.MedicineCatApprentice
            : Cat.CatStatus.Apprentice;

        string oldName = currentCat.fullName;
        currentCat.status = apprenticeType;
        Cat? assignedMentor = AssignMentor(currentCat);

        List<string> involvedCats = new() { currentCat.ID };
        string eventText = $"{oldName} becomes a {currentCat.status.GetEnumDescription()}.";
        if (assignedMentor != null)
        {
            eventText += $" Their mentor is {assignedMentor.fullName}.";
            involvedCats.Add(assignedMentor.ID);
        }
        
        currentEvents.Add(new SingleEvent(eventText, SingleEvent.EventType.Ceremony, timeskips, involvedCats));
    }

    private void GraduateFromApprenticeship(Cat currentCat)
    {
        if (!currentCat.status.IsApprentice()) { return; }

        string oldName = currentCat.fullName;
        string eventText = string.Empty;
        
        if (currentCat.status == Cat.CatStatus.Apprentice)
        {
            currentCat.status = Cat.CatStatus.Warrior;
            eventText = $"{oldName} is ready to become a warrior. A ceremony is held, and they are named " +
                $"{currentCat.fullName}.";
        }
        else if (currentCat.status == Cat.CatStatus.MediatorApprentice)
        {
            currentCat.status = Cat.CatStatus.Mediator;
            eventText = $"{oldName} is ready to become a mediator. A ceremony is held, and they are named " +
                $"{currentCat.fullName}.";
        }
        else if (currentCat.status == Cat.CatStatus.MedicineCatApprentice)
        {
            currentCat.status = Cat.CatStatus.MedicineCat;
            eventText = $"{oldName} is ready to become full medicine cat. A ceremony is held, and they are " +
                $"named {currentCat.fullName}.";
        }
        else { return; }

        currentCat.RemoveMentor();

        currentEvents.Add(new SingleEvent(eventText, SingleEvent.EventType.Ceremony, timeskips, currentCat.ID));
    }
    
    private void Retire(Cat currentCat)
    {
        currentCat.status = Cat.CatStatus.Elder;

        string eventText = $"{currentCat.fullName} retired to elder's den. ";
        currentEvents.Add(new SingleEvent(eventText, SingleEvent.EventType.Ceremony, timeskips, currentCat.ID));
    }

    private Cat? CheckAndPromoteLeader()
    {
        if (currentClan.leader != null) { return null; }
        
        if (currentClan.deputy != null)
        {
            currentClan.deputy.status = Cat.CatStatus.Leader;
        }
        else
        {
            var chosenLeader = SelectValidLeaderOrDeputy(currentClan);
            if (chosenLeader is null)
            {
                return null;
            }

            chosenLeader.status = Cat.CatStatus.Leader;
        }
        
        return currentClan.leader;
    }

    private Cat? CheckAndPromoteDeputy()
    {
        if (currentClan.deputy != null) { return null; }
        
        var chosenDeputy = SelectValidLeaderOrDeputy(currentClan);
        if (chosenDeputy is null)
        {
            return null;
        }

        chosenDeputy.status = Cat.CatStatus.Deputy;

        return chosenDeputy;
    }

    private Cat? SelectValidLeaderOrDeputy(Group clan)
    {
        var allWarriors = clan.GetMembers().Where(x => x.status == Cat.CatStatus.Warrior).ToList();
        var allWarriorsWithApprentices = allWarriors.Where(x => x.apprentices.Count != 0
                                                                || x.previousApprentices.Count != 0).ToList();

        if (allWarriorsWithApprentices.Any())
        {
            return Utilities.ChoseRandom(allWarriorsWithApprentices);
        }
        else if (allWarriors.Any())
        {
            return Utilities.ChoseRandom(allWarriors);
        }

        return null;
    }
      
}