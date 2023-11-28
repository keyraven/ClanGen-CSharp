﻿using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json.Serialization;
using Clangen.Models.CatStuff;

namespace Clangen.Models.CatGroups;

public class OtherClan : Group
{
    [JsonConstructor]
    internal OtherClan() : base(false, new CatDictionary()) { }

    public OtherClan(IReadOnlyFetchableObject<string, Cat> allCats, string prefix) : base( false, allCats)
    {
        this.prefix = prefix;
    }

    public string prefix { get; set; } = string.Empty;
    public string suffix { get; set; } = "Clan";

    public override string GetName()
    {
        return $"{prefix}{suffix}";
    }

    [JsonIgnore]
    public Cat? leader
    {
        get
        {
            return GetMembers().FirstOrDefault(i => i.status == Cat.CatStatus.Leader);
        }
    }

    [JsonIgnore]
    public Cat? deputy
    {
        get
        {
            return GetMembers().FirstOrDefault(i => i.status == Cat.CatStatus.Deputy);
        }
    }

    [JsonIgnore]
    public List<Cat> medicineCats
    {
        get
        {
            return GetMembers().Where(i => i.status == Cat.CatStatus.MedicineCat).ToList();
        }
    }
}