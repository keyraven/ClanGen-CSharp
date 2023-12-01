using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Clangen.Models.CatGroups;
using SkiaSharp;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace Clangen.Models.CatStuff;

public partial class Cat : IEquatable<Cat>
{

    // PRIVATE INSTANCE ATTRIBUTES
    [JsonInclude]
    [JsonPropertyName("relationships")]
    private Dictionary<string, Relationship> _relationships = new();

    [JsonInclude]
    private string belongGroupID => belongGroup.ID;

    private List<string> _previousApprentices = new();
    private int _timeskips = 0;
    private int _experience;
    private List<string> _apprentices = new();
    private List<string> _mates = new();
    private List<string> _previousMates = new();
    private CatStatus _status = CatStatus.Warrior;

    //Only used for JSON loading
    private string _groupIDAtLoadIn = string.Empty;

    //PUBLIC ATTRIBUTES and PROPERTIES

    [JsonPropertyOrder(1)]
    public readonly string ID;

    public Name name { get; set; }

    /// <summary>
    /// Status or Rank of the cat. 
    /// </summary>
    public CatStatus status 
    {
        get
        {
            return _status;
        } 
        set
        {
            _status = value;
            name.nameStatus = value;
        }
    }

    public readonly CatSex sex = CatSex.Female;

    public Pelt pelt { get; set; }
    
    public Personality personality { get; set; }
    
    public CatSkills skills { get; set; }

    public string gender { get; set; } = "female";

    public IReadOnlyList<string> bioParents { get; private set; } = new List<string>();
    
    public List<string> adoptiveParents { get; set; } = new();
    
    public int lives { get; set; } = 1;

    public IReadOnlyList<string> mates
    {
        get { return _mates; }
    }

    public IReadOnlyList<string> previousMates 
    {
        get { return _previousMates; }
    }

    public bool worked { get; set; } = false;

    // Bool if the cat is able to work
    // Injuries are not yet implemented, so always return true.
    [JsonIgnore]
    public bool canWork
    {
        get
        {
            return true;
        }
    }

    [JsonIgnore]
    public string fullName => name.fullName;

    [JsonIgnore]
    public CatAge age { get; private set; }

    [JsonInclude]
    public Backstory backstory { get; private set; } = new(Backstory.BackstoryType.Clanborn);

    [JsonIgnore]
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
    [JsonInclude]
    public string? mentor { get; private set; }

    /// <summary>
    /// List of current apprentices. 
    /// </summary>
    public IReadOnlyCollection<string> apprentices {
        get
        {
            return _apprentices.AsReadOnly();
        }
    }

    public IReadOnlyCollection<string> previousApprentices
    {
        get => _previousApprentices.AsReadOnly();
    }

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

    public List<Pronoun> pronouns { get; set; } = new() {Pronoun.theyThem};

    [JsonIgnore]
    public string thought { get; set; } = "is a cat.";

    [JsonIgnore]
    public ExpLevel experienceLevel { get; private set; } = ExpLevel.Untrained;

    [JsonIgnore]
    public bool dead => belongGroup.dead;

    /// <summary>
    /// Age of the cat, in moons. Readonly, increment timeskips instead. 
    /// </summary>
    [JsonIgnore]
    public float moons
    {
        get
        {
            return (float)timeskips / 2;
        }
    }

    [JsonIgnore]
    public float deadForMoons
    {
        get
        {
            return (float)deadForTimeSkips / 2;
        }
    }

    [JsonIgnore]
    public SKImage sprite { 
        get
        {
            return Sprites.GenerateSprite(this);
        }
    }

    // CONSTRUCTORS
    
    public Cat(string ID, Group belongGroup, CatStatus status = CatStatus.Kit, Pelt? pelt = null, Backstory? backstory = null,
        int timeskips = 0, CatSex sex = CatSex.Female, List<string>? bioParents = null, List<string>? adoptiveParents = null, 
        string? prefix = null, string? gender = null, string? suffix = null, int experience = 0)
    {
        this.ID = ID;
        this.sex = sex;
        this.gender = gender == null ? sex.ToString() : gender;
        this.timeskips = timeskips;
        this.belongGroup = belongGroup;
        this.pelt = pelt == null ? new() : pelt;
        this.backstory = backstory == null ? new Backstory(Backstory.BackstoryType.Clanborn) : backstory;
        this.personality = new("loyal");
        this.skills = new();

        if (prefix == null && suffix == null)
        {
            name = Name.GenerateRandomName(this.status);
        }
        else
        {
            name = new Name(prefix, suffix, this.status);
        }

        this.status = status; //Must be set after name

        if (bioParents != null)
        {
            this.bioParents = bioParents;
        }

        if (adoptiveParents != null)
        {
            this.adoptiveParents = adoptiveParents;
        }
        
        this.experience = experience;

        
    }

    [JsonConstructor]
    internal Cat(string ID, CatSex sex, IReadOnlyList<string> mates, IReadOnlyList<string> previousMates, 
        IReadOnlyList<string> bioParents, Name name, string belongGroupID)
    {
        this.ID = ID;
        this.sex = sex;
        this._mates = mates.ToList();
        this._previousMates = previousMates.ToList();
        this.bioParents = bioParents;
        this.name = name;
        _groupIDAtLoadIn = belongGroupID;

        // These should be set to proper values by the deseralizer after the constructor is called
        pelt = new();
        skills = new();
        personality = new("quiet");


        belongGroup = new Outsiders("0", new CatDictionary());
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
        
    public void SetMate(Cat otherCat)
    {
        throw new NotImplementedException();
    }

    public void UnSetMate(Cat otherCat)
    {
        throw new NotImplementedException();
    }

    public void SetMentor(Cat newMentor)
    {
        this.mentor = newMentor.ID;
        newMentor._apprentices.Add(this.ID);
    }
    
    public void RemoveMentor()
    {
        if (this.mentor is null)
        {
            return;
        }
        
        // We need the worldObject here in order to get the mentor object. 
        // If you can think of a better way to do this, feel free to change. 
        var mentorObject = belongGroup.FetchCat(this.mentor);
        mentorObject._apprentices.Remove(this.ID);
        this.mentor = null;
    }
    
    public bool IsValidMentor(Cat possibleMentor)
    {
        if (this.belongGroup != possibleMentor.belongGroup)
        {
            return false;
        }
        
        switch (status)
        {
            case Cat.CatStatus.Apprentice:
                return possibleMentor.status is Cat.CatStatus.Warrior or Cat.CatStatus.Leader or Cat.CatStatus.Deputy;
            case Cat.CatStatus.MedicineCatApprentice:
                return possibleMentor.status is Cat.CatStatus.MedicineCat;
            case Cat.CatStatus.MediatorApprentice:
                return possibleMentor.status is Cat.CatStatus.Mediator;
            default:
                return false;
        }
    }
    
    public Relationship GetRelationship(string otherCatId)
    {
        if (!_relationships.ContainsKey(otherCatId))
        {
            _relationships.Add(otherCatId, new Relationship(this.ID, otherCatId));
        }

        return _relationships[otherCatId];
    }
    
    public void SetGroupBasedOnDeseralizedGroupID(Func<string, Group> fetchGroup)
    {
        belongGroup = fetchGroup(_groupIDAtLoadIn);
    }
}
