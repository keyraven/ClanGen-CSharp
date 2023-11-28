using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;

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
        General = 0,
        Ceremony = 1 << 0,
    }

    [JsonInclude]
    public string text { get; private set; } = "";

    [JsonInclude]
    public List<string> involvedCats { get; } = new();

    [JsonInclude]
    public EventType types { get; } = EventType.General;

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal SingleEvent() { }
    
    public SingleEvent(string text, EventType types, List<string> involvedCats)
    {
        this.text = text;
        this.involvedCats = new List<string>(involvedCats);
        this.types = types;
    }

    public SingleEvent(string text, params string[] involvedCats)
    {
        this.text = text;
        this.involvedCats = involvedCats.ToList();
    }
    
}