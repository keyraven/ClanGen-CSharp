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
        FormerClanCat,
        [Description("exiled")]
        Exiled
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
        [CatAge.Newborn] = new int[] {0, 1},
        [CatAge.Kitten] = new int[] {2, 11},
        [CatAge.Adolescent] = new int[] {12, 23},
        [CatAge.YoungAdult] = new int[] {24, 95},
        [CatAge.Adult] = new int[] {96, 191},
        [CatAge.SeniorAdult] = new int[] {192, 239},
        [CatAge.Senior] = new int[] {240, 600}
     };
    
}