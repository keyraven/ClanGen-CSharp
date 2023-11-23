using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Clangen.Models.CatGroups;
using SkiaSharp;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Clangen.Models.CatStuff;

public partial class Cat : IEquatable<Cat>
{

    // PRIVATE INSTANCE ATTRIBUTES
    private CatStatus _status = new();
    private Dictionary<string, Relationship> _relationships { get; set; } = new();
    private int _timeskips = 0;
    private int _experience;
    

    //PUBLIC ATTRIBUTES and PROPERTIES
    
    public readonly string ID;

    public Name name { get; set; }
    
    public string fullName
    {
        get { return name.fullName; }
    }
    
    public Pelt pelt { get; set; }
    
    public Personality personality { get; set; }
    
    public CatSkills skills { get; set; }
    
    public readonly CatSex Sex;
    
    public string gender { get; set; }
    
    public readonly IReadOnlyList<string> bioParents = new List<string>();
    
    public List<string> adoptiveParents { get; set; } = new();
    
    public int lives { get; set; } = 1;
    
    public List<string> mates { get; private set; } = new();
    
    public List<string> previousMates { get; private set; } = new();
    
    public CatAge age { get; private set; }
    
    public bool worked { get; set; } = false;
    
    public Group belongGroup { get; set; }
    
    /// <summary>
    /// Number of timeskips a cat has gone through
    /// 1 Timeskip = 1/2 moon.
    /// </summary>
    public int timeskips
    {
        get
        {
            return _timeskips;
        }
        set
        {
            _timeskips = value;
            
            // Set Age
            bool found = false;
            foreach (var item in Cat.AgeTimeskips)
            {
                if (item.Value[0] <= _timeskips && _timeskips <= item.Value[1])
                {
                    age = item.Key;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                // If not found, it is above the possible ranges. 
                // Set to senior
                age = CatAge.Senior;
            }
        }
    }
    
    public int deadForTimeSkips { get; set; } = 0;
    
    /// <summary>
    /// Cat's mentor. Can be null to indicate that a cat doesn't have a mentor
    /// </summary>
    public string? mentor { get; private set; }

    /// <summary>
    /// List of current apprentices. 
    /// </summary>
    public List<string> apprentice { get; private set; } = new();
    
    public List<string> previousApprentices { get; set; } = new();
    
    public int experience
    {
        get { return _experience; }
        set
        {
            _experience = value;
            //TODO - properly set EXP level
            experienceLevel = ExpLevel.Untrained;
        }
    }
    
    public string thought { get; set; } = "is a cat.";
    public List<Pronoun> pronouns { get; set; } = new() {Pronoun.theyThem};
    public ExpLevel experienceLevel { get; set; } = ExpLevel.Untrained;
    
    public bool dead
    {
        get { return belongGroup.dead; }
    }
    
    
    /// <summary>
    /// Age of the cat, in moons. Readonly, increment timeskips instead. 
    /// </summary>
    public float moons
    {
        get
        {
            return (float)timeskips / 2;
        }
    }

    public float deadForMoons
    {
        get
        {
            return (float)deadForTimeSkips / 2;
        }
    }
    
    public SKImage sprite { get; set; }

    // Bool if the cat is able to work
    // Injuries are not yet implemented, so always return true.
    public bool canWork
    {
        get
        {
            return true;
        }
    }
    
    /// <summary>
    /// Status or Rank of the cat. 
    /// </summary>
    public CatStatus status
    {
        get { return _status; }
        set
        {
            CatStatus oldStatus = _status;
            _status = value;
        }
    }
    
    public Cat(string id, Group belongGroup = null, CatStatus status = CatStatus.Kit, Pelt? pelt = null,
        int timeskips = 0, CatSex sex = CatSex.Female, List<string>? bioParents = null, List<string>? adoptiveParents = null, 
        string? prefix = null, string? gender = null, string? suffix = null, int experience = 0)
    {
        this.ID = id;
        this.Sex = sex;
        this.gender = gender == null ? sex.ToString() : gender;
        this.status = status;
        this.timeskips = timeskips;
        // ToDo - Add protection to prevent putting a cat in group that doesn't belong to it's world. 
        this.belongGroup = belongGroup;
        this.pelt = pelt == null ? new() : pelt;
        
        if (bioParents != null)
        {
            this.bioParents = bioParents;
        }

        if (adoptiveParents != null)
        {
            this.adoptiveParents = adoptiveParents;
        }
        
        this.experience = experience;

        if (prefix == null && suffix == null)
        {
            name = Name.GenerateRandomName(this);
        }
        else
        {
            name = new Name(prefix, suffix, cat: this);
        }
        
        this.sprite = Sprites.GenerateSprite(this);
    }
    
    // EQ-OVERRIDES (And HASH)

    public override bool Equals(object? obj) => this.Equals(obj as Cat);

    public bool Equals(Cat? p)
    {
        if (p is null)
        {
            return false;
        }

        if (Object.ReferenceEquals(this, p))
        {
            return true;
        }

        if (this.GetType() != p.GetType())
        {
            return false;
        }

        // Cat is the same if the IDs are equal. 
        return this.ID == p.ID;
    }

    public override int GetHashCode() => ID.GetHashCode();

    public static bool operator ==(Cat? lhs, Cat? rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                return true;
            }

            // Only the left side is null.
            return false;
        }
        // Equals handles case of null on right side.
        return lhs.Equals(rhs);
    }

    public static bool operator !=(Cat? lhs, Cat? rhs) => !(lhs == rhs);

    // END EQ OVERRIDES
    
    public void TimeSkip()
    {
        timeskips += 1;
    }
    
    public bool IsPotentialMate(Cat otherCat)
    {
        if (this == otherCat)
        {
            return false;
        }

        return true;
    }
    
    public void SetMate(Cat otherCat)
    {
        throw new NotImplementedException();
    }

    public void UnSetMate(Cat otherCat)
    {
        throw new NotImplementedException();
    }

    public void SetApprentice(Cat apprentice)
    {
        throw new NotImplementedException();
    }
    
    public Relationship GetRelationship(string otherCatId)
    {
        if (!_relationships.ContainsKey(otherCatId))
        {
            _relationships.Add(otherCatId, new Relationship(this.ID, otherCatId));
        }

        return _relationships[otherCatId];
    }
    

}
