using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Clangen.Models.CatStuff;

public class Pronoun
{
    public enum Conjugation
    {
        Plural = 1,
        Singular = 2
    }
    
    //Default pronouns.
    public static readonly Pronoun theyThem = new
        Pronoun("they", "them", "their", "theirs", "themself", Conjugation.Plural);
    public static readonly Pronoun sheHer = new Pronoun(
        "she", "her", "her", "hers", "herself", Conjugation.Singular);
    public static readonly Pronoun heHim = new Pronoun(
        "he", "him", "his", "his", "himself", Conjugation.Singular);
    public static readonly List<Pronoun> DefaultPronouns = new() { theyThem, sheHer, heHim };

    
    public string objectVersion { get; set; } = string.Empty;
    public string subjectVersion { get; set; } = string.Empty;
    public string possessiveVersion { get; set; } = string.Empty;
    public string inPossessiveVersion { get; set; } = string.Empty;
    public string reflexiveVersion { get; set; } = string.Empty;
    public Conjugation conjugation { get; set; } = Conjugation.Singular;

    [JsonConstructor]
    internal Pronoun() { }

    public Pronoun(string objectt, string subject, string possessive, string inPossessive, string reflexive,
        Conjugation conjugation)
    {
        this.objectVersion = objectt;
        this.subjectVersion = subject;
        this.possessiveVersion = possessive;
        this.inPossessiveVersion = inPossessive;
        this.reflexiveVersion = reflexive;
        this.conjugation = conjugation;
    }

    /// <summary>
    /// Return the pronoun indicated by the conjugation type string. 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string GetPronoun(string type)
    {
        switch (type)
        {
            case "object":
                return objectVersion;
            case "subject":
                return subjectVersion;
            case "poss":
                return possessiveVersion;
            case "inposs":
                return inPossessiveVersion;
            case "self":
                return reflexiveVersion;
            default:
                return $"error--\"{type}\"";
        }
    }
}
