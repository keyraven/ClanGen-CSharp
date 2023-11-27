using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Clangen.Models.CatStuff;

namespace Clangen.Models.CatGroups;

public class OtherClan : Group
{
    public OtherClan(IReadOnlyFetchableObject<string, Cat> allCats, string prefix) : base( false, allCats)
    {
        this.prefix = prefix;
    }

    public string prefix { get; set; }
    public string suffix { get; set; } = "Clan";

    public override string GetName()
    {
        return $"{prefix}{suffix}";
    }
    
    public Cat? leader
    {
        get
        {
            return GetMembers().FirstOrDefault(i => i.status == Cat.CatStatus.Leader);
        }
    }

    public Cat? deputy
    {
        get
        {
            return GetMembers().FirstOrDefault(i => i.status == Cat.CatStatus.Deputy);
        }
    }
    public List<Cat> medicineCats
    {
        get
        {
            return GetMembers().Where(i => i.status == Cat.CatStatus.MedicineCat).ToList();
        }
    }
}