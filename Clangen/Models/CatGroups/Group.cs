﻿using System.Collections.Generic;
using Clangen.Models.CatStuff;

namespace Clangen.Models.CatGroups;

public abstract class Group
{
    public readonly string ID = GetValidId();

    private static int _lastId = 1;
    
    private static string GetValidId()
    {
        _lastId += 1;
        while (true)
        {
            if (Game.currentWorld == null)
            {
                break;
            }

            if (Game.currentWorld.AllCats.ContainsKey(_lastId.ToString()) |
                Game.currentWorld.FadedIds.Contains(_lastId.ToString()))
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
    
    public abstract string name { get; }

    public void AddMember(Cat newCat)
    {
        newCat.belongGroup?.RemoveMember(newCat);
        members.Add(newCat.Id);
        newCat.belongGroup = this;
    }

    public void RemoveMember(Cat removeCat)
    {
        members.Remove(removeCat.Id);
        removeCat.belongGroup = null;
    }

    public SortedSet<string> members { get; set; } = new();
}