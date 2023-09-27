using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Kitten,
        [Description("newborn")]
        Newborn,
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

    // ID Stuff

    private static int _lastId = 0;

    /// <summary>
    /// Returns a valid, open ID. If a currentClan is set in Game,
    /// it will check that that ID is not current held by a cat in that clan.
    /// Otherwise, simply increments get the next integer from private integer _lastID
    /// </summary>
    /// <returns>String, a valid ID. </returns>
    private static string GetValidId()
    {
        _lastId += 1;
        while (true)
        {
            if (Game.CurrentClan == null)
            {
                break;
            }

            if (Game.CurrentClan.AllCats.ContainsKey(_lastId.ToString()) |
                    Game.CurrentClan.FadedIds.Contains(_lastId.ToString()))
            {
                _lastId += 1;
            }
            else
            {
                break;
            }
        }

        return _lastId.ToString();
    }


    // STATIC PROPERTIES
    public static readonly Dictionary<CatAge, int[]> AgeMoons = Game.gameConfig.ageMoons;

}