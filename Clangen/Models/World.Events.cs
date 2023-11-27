using System.Collections.Generic;
using Clangen.Models.CatStuff;
using Clangen.Models.Events;

namespace Clangen.Models;

// This is for various timeskip actions
// Note- maybe move to it's own class. 

public partial class World
{

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

    private void AliveClanCatTimeskip(Cat currentCat)
    {
        currentCat.timeskips++;
        
        // Check mentor is still valid, reassign if not. 
        CheckAndAdjustMentor(currentCat);
        
        // Update and Check Status, Preform Ceremonies
        CheckAndUpdateStatus(currentCat);
        
    }

    private void DeadCatTimeSkip(Cat currentCat)
    {
        if (catsDeadOnCurrentTimeSkip.Contains(currentCat.ID))
        {
            currentCat.timeskips++;
        }
        else
        {
            currentCat.deadForTimeSkips++;
        }
    }

    private void OutsideCatTimeSkip(Cat currentCat)
    {
        currentCat.timeskips++;
    }

    private void OtherCatTimeSkip(Cat currentCat)
    {
        currentCat.timeskips++;
    }
    

    private void CheckAndUpdateStatus(Cat currentCat)
    {
        // Updating Status Kit --> Apprentice
        if (currentCat.timeskips == Cat.AgeTimeskips[Cat.CatAge.Adolescent][0])
        {
            MakeApprentice(currentCat);
        }
        // Updating Status Apprentice --> 
        else if (currentCat.status.IsApprentice() && currentCat.timeskips == Cat.AgeTimeskips[Cat.CatAge.YoungAdult][0])
        {
            GraduateFromApprenticeship(currentCat);
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
        
        currentEvents.Add(new SingleEvent(eventText, SingleEvent.EventType.Ceremony, involvedCats));
    }

    private void GraduateFromApprenticeship(Cat currentCat)
    {
        if (!currentCat.status.IsApprentice())
        {
            return;
        }
        
        
    }
    
}