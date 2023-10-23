using System.Collections.Generic;
using System.Linq;
using Clangen.Models.CatStuff;

namespace Clangen.Models.CatGroups;

public abstract class Group
{
    public readonly string ID = GetValidId();
    private static int _lastId = 0;
    private IReadOnlyDictionary<string, Cat> _allCats { get; }

    protected Group(IReadOnlyDictionary<string, Cat> allCats)
    {
        _allCats = allCats;
    }

    public IReadOnlyCollection<Cat> GetMembers()
    {
        return _allCats.Values.Where(cat => cat.belongGroup == this).ToList().AsReadOnly();
    }
    
    private static string GetValidId()
    {
        _lastId += 1;
        while (true)
        {
            if (Game.currentWorld == null)
            {
                break;
            }

            if (Game.currentWorld.allGroups.ContainsKey(_lastId.ToString()))
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

    public abstract string GetName();

    public abstract void UpdateCatStatus(Cat cat);
}