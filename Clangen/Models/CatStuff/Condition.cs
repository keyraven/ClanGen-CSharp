namespace Clangen.Models.CatStuff;

public class Condition
{
    public int duration { get; private set; } = 1;
    public int lethality { get; private set; } = -1;
    public bool preventWorking { get; } = false;
    public int timeskips { get; private set; } = 0;
}