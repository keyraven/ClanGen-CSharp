namespace Clangen.Models.CatStuff;

public enum SkillPath
{
    Swim,
    Hunt
}


public class Skill
{
    public SkillPath Path;
    public int Points;
    public bool InterestOnly;

    public Skill(SkillPath path, int points = 0, bool interestOnly = false)
    {
        Path = path;
        Points = points;
        InterestOnly = interestOnly;
    }
}

public class CatSkills
{
    public Skill? Primary;
    public Skill? Secondary;
    public Skill? Hidden;

    public CatSkills(Skill? primary = null, Skill? secondary = null, Skill? hidden = null)
    {
        Primary = primary;
        Secondary = secondary;
        Hidden = hidden;
    }

}