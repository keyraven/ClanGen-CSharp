using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clangen.Models.CatStuff;

using System.Linq;
using System.Net.Mail;

public enum RelationType
{
    Blood,
    Adoptive,
    HalfBlood,
    NotBlood,
    Related
}

/// <summary>
/// Holds relation, for use as a dictionary value. 
/// </summary>
public class RelationInfo
{
    public RelationType Type;
    public List<string> Additional;

    public RelationInfo(RelationType type = RelationType.Blood, List<string>? additional = null)
    {
        additional ??= new List<string>();

        Type = type;
        Additional = additional;
    }
}


public class Inheritance
{

    public Inheritance(Cat cat, bool born = false)
    {
        this.cat = cat;
        Update();
        if (born)
        {

        }
    }

    public Cat cat { get; set; }

    public Dictionary<string, RelationInfo> parents { get; private set; } = new();
    public Dictionary<string, RelationInfo> mates { get; private set; } = new();
    public Dictionary<string, RelationInfo> kits { get; private set; } = new();
    public Dictionary<string, RelationInfo> kitsMates { get; private set; } = new();
    public Dictionary<string, RelationInfo> siblings { get; private set; } = new();
    public Dictionary<string, RelationInfo> siblingsMates { get; private set; } = new();
    public Dictionary<string, RelationInfo> siblingsKits { get; private set; } = new();
    public Dictionary<string, RelationInfo> parentsSiblings { get; private set; } = new();
    public Dictionary<string, RelationInfo> cousins { get; private set; } = new();
    public Dictionary<string, RelationInfo> grandParents { get; private set; } = new();
    public Dictionary<string, RelationInfo> grandKits { get; private set; } = new();
    public List<string> allInvolved { get; private set; } = new();


    public void Update()
    {

        if (Game.CurrentClan == null)
        {
            throw new Exception("Can't update inheritance without clan loaded.");
        }

        parents.Clear();
        mates.Clear();
        kits.Clear();
        kitsMates.Clear();
        siblings.Clear();
        siblingsMates.Clear();
        siblingsKits.Clear();
        parentsSiblings.Clear();
        cousins.Clear();
        grandParents.Clear();
        grandKits.Clear();

        //Parents
        InitParents();





    }

    private void InitParents()
    {
        Cat? current;
        //Blood
        foreach (var id in cat.bioParents)
        {
            current = Game.CurrentClan.FetchCat(id);
            if (current == null)
            {
                continue;
            }

            parents.Add(id, new RelationInfo(type: RelationType.Blood));
            allInvolved.Add(id);
        }

        //Adoptive
        foreach (var id in cat.adoptiveParents)
        {
            current = Game.CurrentClan.FetchCat(id);
            if (current == null)
            {
                continue;
            }

            parents.Add(id, new RelationInfo(type: RelationType.Adoptive));
            allInvolved.Add(id);
        }
    }

    private void InitMates()
    {
        foreach (var innerId in cat.mates)
        {
            RelationType mateRel = allInvolved.Contains(innerId) ? RelationType.Related : RelationType.NotBlood;
            mates.Add(innerId, new RelationInfo(type: mateRel, additional: new() { "current mate" }));
        }

        foreach (var innerId in cat.previousMates)
        {
            RelationType mateRel = allInvolved.Contains(innerId) ? RelationType.Related : RelationType.NotBlood;
            mates.Add(innerId, new RelationInfo(type: mateRel, additional: new() { "previous mate" }));
        }
    }

    private void InitGrandParents()
    {

    }

    private List<string> GetAllParents()
    {
        return cat.bioParents.Concat(cat.adoptiveParents).ToList();
    }

}
