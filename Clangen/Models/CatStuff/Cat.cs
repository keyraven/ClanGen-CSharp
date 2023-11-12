using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using Clangen.Models.CatGroups;
using SkiaSharp;

namespace Clangen.Models.CatStuff;

public partial class Cat : IEquatable<Cat>
{

    // PRIVATE INSTANCE ATTRIBUTES
    private CatStatus _status = new();
    private int _moons;
    private string? _mentor;
    private List<string> _apprentices = new();
    

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
    public List<Pronoun> pronouns { get; set; } = new() {Pronoun.theyThem};
    public readonly List<string> bioParents;
    public List<string> adoptiveParents { get; set; }
    public bool darkForest { get; set; }
    public bool dead { get; private set; }
    public int lives { get; private set; } = 1;
    public Dictionary<string, Relationship> relationships { get; set; } = new();
    public List<string> mates { get; private set; } = new();
    public List<string> previousMates { get; private set; } = new();
    public CatAge age { get; private set; }
    
    // True if the cat worked in the last timeskip, false if not. 
    public bool worked { get; set; } = false;

    public string thought { get; set; } = "is a cat.";
    
    private int _timeskips = 0;
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

    public int deadForTimeSkips { get; set; } = 0;

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

    // TODO-- proper setters.
    /// <summary>
    /// Cat's mentor. Can be null to indicate that a cat doesn't have a mentor
    /// </summary>
    public string? mentor
    {
        get { return _mentor; }
        private set { _mentor = value; }
    }
    
    /// <summary>
    /// List of current apprentices. 
    /// </summary>
    public List<string> apprentice
    {
        get { return _apprentices; }
        private set { _apprentices = value; }
    }
    
    public List<string> previousApprentices { get; set; } = new();
    
    private int _experience;
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
    
    public ExpLevel experienceLevel { get; set; }
    public Group belongGroup { get; set; }
    public readonly World belongWorld;

    public Cat(string id, World belongWorld, Group? belongGroup = null, CatStatus status = CatStatus.Kit, 
        int timeskips = 0, CatSex sex = CatSex.Female, List<string>? bioParents = null, List<string>? adoptiveParents = null, 
        string? prefix = null, string? gender = null, string? suffix = null, int experience = 0)
    {
        this.ID = id;
        this.Sex = sex;
        this.gender = gender == null ? sex.ToString() : gender;
        this.status = status;
        this.timeskips = timeskips;
        this.belongWorld = belongWorld;
        // ToDo - Add protection to prevent putting a cat in group that doesn't belong to it's world. 
        this.belongGroup = belongGroup == null ? belongWorld.currentClan : belongGroup;

        pelt = new();
        
        belongWorld.AddCatToWorld(this);
        
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
    
    /// <summary>
    /// Kills the kitty
    /// </summary>
    /// <returns> Some extra event text.  </returns>
    public string Die()
    {
        string output = "";
        lives -= 1;
        if (lives < 1)
        {
            dead = true;
        }
        else
        {
            output = $"{fullName} lost a life.";
        }

        return output;
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
    
    // RELATION CHECKING FUNCTIONS

    public bool IsRelated(Cat otherCat)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Check if this cat is the parent of otherCat.
    /// </summary>
    /// <param name="otherCat"></param>
    public bool IsParentOf(Cat otherCat)
    {
        return otherCat.IsChildOf(this);
    }

    /// <summary>
    /// Check if this cat is the child of otherCat
    /// </summary>
    /// <param name="otherCat"></param>
    /// <returns></returns>
    public bool IsChildOf(Cat otherCat)
    {
        return (this.bioParents.Contains(otherCat.ID) || this.adoptiveParents.Contains(otherCat.ID));
    }
    
    /// <summary>
    /// Check is this cat and otherCat are siblings. 
    /// </summary>
    /// <param name="otherCat"></param>
    /// <returns></returns>
    public bool IsSibling(Cat otherCat)
    {
        IEnumerable<string> allParents = bioParents.Concat(adoptiveParents);
        return allParents.Intersect(otherCat.bioParents.Concat(otherCat.adoptiveParents)).Any();
    }

    
    public bool IsSiblingsChildOf(Cat otherCat)
    {
        foreach (string parentID in bioParents.Concat(adoptiveParents))
        {
            Cat parent = belongWorld.FetchCat(parentID);
            if (parent.IsSibling(otherCat))
            {
                return true;
            }
        }

        return false;
    }
    

}
