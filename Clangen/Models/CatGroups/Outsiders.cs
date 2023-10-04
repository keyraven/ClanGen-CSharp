namespace Clangen.Models.CatGroups;

public class Outsiders : Group
{
    public override string name { get; }

    public Outsiders(string name = "Cats Outside the Clan")
    {
        this.name = name;
    }
}