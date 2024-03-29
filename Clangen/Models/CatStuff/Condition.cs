﻿namespace Clangen.Models.CatStuff;

public class Condition
{
    public int duration { get; private set; } = 1;
    public int lethality { get; private set; } = -1;
    public bool preventWorking { get; } = false;
    public int timeskips { get; private set; } = 0;
}

public class Injury : Condition
{
    public Complication? complication { get; set; }
}

public class Illness : Condition
{
    public Complication? complication { get; set; }
}

public class PermCondition : Condition
{
    public Complication? complication { get; set; }
}

public class Complication : Condition
{
    
}
