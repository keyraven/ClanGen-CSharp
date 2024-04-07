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
    public string text { get; init; } = "";

    [JsonInclude]
    public List<string> involvedCats { get; init; } = new();

    [JsonInclude]
    public EventType types { get; init; } = EventType.General;

    [JsonInclude] 
    public int timeskip { get; init; } = 0;

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal SingleEvent() { }
    
    public SingleEvent(string text, EventType types, int timeskip, List<string> involvedCats)
    {
        this.text = text;
        this.involvedCats = involvedCats;
        this.types = types;
        this.timeskip = timeskip;
    }

    public SingleEvent(string text, EventType types, int timeskip, params string[] involvedCats)
    {
        this.text = text;
        this.involvedCats = involvedCats.ToList();
        this.types = types;
        this.timeskip = timeskip;
    }
    
}