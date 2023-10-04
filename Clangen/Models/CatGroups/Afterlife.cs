using System.Collections.Generic;

namespace Clangen.Models.CatGroups;

public class Afterlife : Group
{
    public override string name { get; }
    
    public Afterlife(string name = "Afterlife", SortedSet<string>? cats = null)
    {
        this.name = name;
        this.members = cats == null ? new SortedSet<string>() : cats;
    }
}