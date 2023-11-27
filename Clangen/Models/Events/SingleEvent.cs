using System;
using System.Collections.Generic;
using System.Linq;

namespace Clangen.Models.Events;

/// <summary>
/// Holds information on a single event, generated during timeskip.
/// This is a simple class that holds some event text, and an array
/// of involved cat IDs. 
/// </summary>
public class SingleEvent
{
    [Flags]
    public enum EventType
    {
        Ceremony = 0,
        BirthAndDeath = 1 << 0,
    }
    
    public string text { get; private set; }

    public string[] involvedCats { get; private set; }

    public SingleEvent(string text, EventType types, List<string> involvedCats)
    {
        this.text = text;
        this.involvedCats = involvedCats.ToArray();
    }

    public SingleEvent(string text, params string[] involvedCats)
    {
        this.text = text;
        this.involvedCats = involvedCats;
    }
    
}