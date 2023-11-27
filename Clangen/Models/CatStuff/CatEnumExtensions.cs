using System;
using System.ComponentModel;
using System.Reflection;

namespace Clangen.Models.CatStuff;

public static class CatEnumExtensions
{
    public static bool IsApprentice(this Cat.CatStatus status)
    {
        return (status is Cat.CatStatus.Apprentice or Cat.CatStatus.MediatorApprentice
            or Cat.CatStatus.MedicineCatApprentice);
    }

    public static bool CanRetire(this Cat.CatStatus status)
    {
        return (status is Cat.CatStatus.Warrior or Cat.CatStatus.Deputy);
    }

    public static bool IsMedical(this Cat.CatStatus status)
    {
        return (status is Cat.CatStatus.MedicineCatApprentice or Cat.CatStatus.MedicineCat);
    }

    public static bool IsAdult(this Cat.CatAge age)
    {
        return !(age is Cat.CatAge.Kitten or Cat.CatAge.Adolescent);
    }
    
}