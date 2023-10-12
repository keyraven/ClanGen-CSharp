using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
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

    public readonly string Id = GetValidId();
    
    public Name name { get; set; }
    public string fullName
    {
        get { return name.fullName; }
    }

    public Pelt pelt { get; set; } = new();
    public Personality personality { get; set; }
    public CatSkills skills { get; set; }
    public readonly CatSex Sex;
    public string gender { get; set; }
    public List<Pronoun> pronouns { get; set; } = new() {Pronoun.theyThem};
    public readonly List<string> bioParents;
    public List<string> adoptiveParents { get; set; }
    public bool outside { get; set; }
    public bool darkForest { get; set; }
    public bool dead { get; private set; }
    public int lives { get; private set; } = 1;
    public Dictionary<string, Relationship> relationships { get; set; } = new();
    public List<string> mates { get; private set; } = new();
    public List<string> previousMates { get; private set; } = new();
    public CatAge age { get; private set; }
    
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
    
    public SKImage sprite { get; set; }

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
    
    /// <summary>
    /// Current experience level, as an enum. 
    /// </summary>
    public ExpLevel experienceLevel { get; set; }
    
    // TODO - figure out how to handle sprites. IDEA - use SKSharp. 
    //public SKBitmap sprite { get; set; }
    
    // Group the cat belongs too. 
    public Group? belongGroup { get; set; }

    public Cat(CatStatus status = CatStatus.Newborn, int timeskips = 0, CatSex sex = CatSex.Female,
        List<string>? bioParents = null, List<string>? adoptiveParents = null, string? prefix = null,
        string? gender = null, string? suffix = null, int experience = 0)
    {
        
        this.Sex = sex;
        this.gender = gender == null ? sex.ToString() : gender;
        this.status = status;
        this.timeskips = timeskips;
        if (bioParents != null)
        {
            this.bioParents = bioParents;
        }

        if (adoptiveParents != null)
        {
            this.adoptiveParents = adoptiveParents;
        }
        this.experience = experience;

        name = new Name(prefix, suffix, cat: this);

        this.sprite = Sprites.GenerateSprite(this);
    }

    //NEW CAT FUNCTIONS

    public static Cat GenerateRandomCat()
    {
        return new Cat();
    }


    // EQ-OVERRIDES

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
        return this.Id == p.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

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
    
    /// <summary>
    /// Makes the cat a mate
    /// </summary>
    /// <param name="otherCat"></param>
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
    
    // Relation Functions

    public bool IsRelated(Cat otherCat)
    {
        throw new NotImplementedException();
    }

}
