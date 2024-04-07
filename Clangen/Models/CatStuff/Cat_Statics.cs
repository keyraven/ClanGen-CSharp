using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Clangen.Models.CatStuff;

public partial class Cat : IEquatable<Cat>
{
    public enum CatStatus
    {
        [Description("medicine cat")]
        MedicineCat,
        [Description("warrior")]
        Warrior,
        [Description("leader")]
        Leader,
        [Description("medicine cat apprentice")]
        MedicineCatApprentice,
        [Description("apprentice")]
        Apprentice,
        [Description("kitten")]
        Kit,
        [Description("deputy")]
        Deputy,
        [Description("elder")]
        Elder,
        [Description("mediator")]
        Mediator,
        [Description("mediator apprentice")]
        MediatorApprentice,
        [Description("kittypet")]
        Kittypet,
        [Description("loner")]
        Loner,
        [Description("rogue")]
        Rogue,
        [Description("former Clancat")]
        FormerClanCat
    }

    public enum CatSecondaryStatus
    {
        [Description("None")]
        None,
        [Description("exiled")]
        Exiled,
        [Description("lost")]
        Lost
    }
    
    public enum CatAge
    {
        [Description("newborn")]
        Newborn,
        [Description("kitten")]
        Kitten,
        [Description("adolescent")]
        Adolescent,
        [Description("young adult")]
        YoungAdult,
        [Description("adult")]
        Adult,
        [Description("senior adult")]
        SeniorAdult,
        [Description("senior")]
        Senior
    }
    public enum CatSex
    {
        [Description("female")]
        Female,
        [Description("male")]
        Male
    }

    public enum ExpLevel
    {
        [Description("untrained")]
        Untrained,
        [Description("trainee")]
        Trainee,
        [Description("prepared")]
        Prepared,
        [Description("competent")]
        Competent,
        [Description("Proficient")]
        Proficient,
        [Description("expert")]
        Expert,
        [Description("master")]
        Master
    }
    
    //Reference

    public static readonly IReadOnlyDictionary<CatAge, int[]> AgeTimeskips = new Dictionary<CatAge, int[]>()
    {
        [CatAge.Newborn] = new int[] {0, 3},
        [CatAge.Kitten] = new int[] {4, 23},
        [CatAge.Adolescent] = new int[] {24, 47},
        [CatAge.YoungAdult] = new int[] {48, 191},
        [CatAge.Adult] = new int[] {192, 383},
        [CatAge.SeniorAdult] = new int[] {384, 479},
        [CatAge.Senior] = new int[] {480, 1200}
     };
    
}