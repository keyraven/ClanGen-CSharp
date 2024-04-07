using System;
using System.Collections.Generic;
using Clangen.Models.CatGroups;
using SkiaSharp;
using System.Text.Json.Serialization;

namespace Clangen.Models.CatStuff;

public partial class Cat : IEquatable<Cat>
{
    
    // PRIVATE INSTANCE ATTRIBUTES
    private int _timeskips = 0;
    private int _experience = 0;
    
    [JsonInclude]
    [JsonPropertyName("status")]
    private CatStatus _status = CatStatus.Warrior;
    
    [JsonInclude]
    [JsonPropertyName("secondaryStatus")]
    private CatSecondaryStatus _secondaryStatus = CatSecondaryStatus.None;

    [JsonInclude]
    [JsonPropertyName("relationships")]
    private Dictionary<string, Relationship> _relationships = new();

    [JsonInclude]
    private string belongGroupID => belongGroup.ID;

    //Only used for JSON loading
    private string _groupIDAtLoadIn = string.Empty;

    [JsonInclude]
    [JsonPropertyName("mates")]
    private List<string> _mates = new();
    
    [JsonInclude]
    [JsonPropertyName("previousMates")]
    private List<string> _previousMates = new();

    [JsonInclude]
    [JsonPropertyName("apprentices")]
    private List<string> _apprentices = new();

    [JsonInclude]
    [JsonPropertyName("previousApprentices")]
    private List<string> _previousApprentices = new();

    //PUBLIC ATTRIBUTES and PROPERTIES
    
    public readonly string ID;

    [JsonRequired]
    public Name name { get; set; }

    /// <summary>
    /// Status or Rank of the cat. 
    /// </summary>
    [JsonIgnore]
    public CatStatus status 
    {
        get
        {
            return _status;
        } 
        set
        {
            // Prevent doing tasks twice for the same status value.
            // There are som, like life settings, they should only be done once. 
            if (_status != value)
            {
                _status = value;
                name.nameStatus = value;

                switch (_status)
                {
                    case CatStatus.Leader:
                        lives = 9;
                        break;
                }
            }
        }
    }

    [JsonIgnore]
    public CatSecondaryStatus secondaryStatus
    {
        get
        {
            return _secondaryStatus;
        }
        set
        {
            _secondaryStatus = value;
        }
    }

    public readonly CatSex sex = CatSex.Female;

    public Pelt pelt { get; set; }
    
    public Personality personality { get; set; }
    
    public CatSkills skills { get; set; }

    public string gender { get; set; } = "female";

    [JsonInclude]
    public IReadOnlyList<string> bioParents { get; init; } = new List<string>();
    
    public List<string> adoptiveParents { get; set; } = new();
    
    public int lives { get; set; } = 1;

    public bool worked { get; set; } = false;

    [JsonInclude]
    public Backstory backstory { get; private set; } = new(Backstory.BackstoryType.Clanborn);

    /// <summary>
    /// Number of timeskips a cat has gone through
    /// 1 Timeskip = 1/2 moon.
    /// </summary>
    [JsonRequired]
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
    
    [JsonRequired]
    public int deadForTimeSkips { get; set; } = 0;

    [JsonInclude]
    public string? mentor { get; private set; }
    
    public string playerNotes { get; set; } = String.Empty;
    
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

    [JsonIgnore] // For now. 
    public List<Pronoun> pronouns { get; set; } = new() {Pronoun.theyThem};

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

    [JsonIgnore]
    public Group belongGroup { get; set; }

    [JsonIgnore]
    public IReadOnlyList<string> previousMates
    {
        get { return _previousMates; }
    }

    [JsonIgnore]
    public IReadOnlyList<string> mates
    {
        get { return _mates; }
    }

    [JsonIgnore]
    public IReadOnlyCollection<string> apprentices
    {
        get
        {
            return _apprentices.AsReadOnly();
        }
    }

    [JsonIgnore]
    public IReadOnlyCollection<string> previousApprentices
    {
        get => _previousApprentices.AsReadOnly();
    }

    [JsonIgnore]
    public string thought { get; set; } = "is a cat.";

    [JsonIgnore]
    public ExpLevel experienceLevel { get; private set; } = ExpLevel.Untrained;

    [JsonIgnore]
    public bool dead => belongGroup.dead;

    [JsonIgnore]
    public bool faded { get; set; } = false;

    /// <summary>
    /// Age of the cat, in moons. Readonly, increment timeskips instead. 
    /// </summary>
    [JsonIgnore]
    public float moons
    {
        get
        {
            return (float)timeskips / 4;
        }
    }

    [JsonIgnore]
    public float deadForMoons
    {
        get
        {
            return (float)deadForTimeSkips / 4;
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
        this.pelt = pelt ?? new();
        this.backstory = backstory ?? new Backstory(Backstory.BackstoryType.Clanborn);
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
    //Never use. Only for deserialization
    internal Cat(string ID, CatSex sex, Name name, string belongGroupID)
    {
        this.ID = ID;
        this.sex = sex;
        this.name = name;
        _groupIDAtLoadIn = belongGroupID;

        // These should be set to proper values by the deserializer after the constructor is called
        pelt = new();
        skills = new();
        personality = new("quiet");

        // Dummy group, to be replaced with true group late during the load-in process 
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
        otherCat._mates.Add(this.ID);
        this._mates.Add(otherCat.ID);
    }

    public void RemoveMate(Cat otherCat)
    {
        if (!_mates.Contains(otherCat.ID)) { return; }

        _mates.Remove(otherCat.ID);
        otherCat._mates.Remove(this.ID);
    }

    public void RemoveMate(string catID)
    {
        if (!_mates.Contains(catID)) { return; }

        belongGroup.FetchCat(catID)._mates.Remove(this.ID);
        _mates.Remove(this.ID);
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
