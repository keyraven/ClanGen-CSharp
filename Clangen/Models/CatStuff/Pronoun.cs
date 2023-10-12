using System.Collections.Generic;

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


    // this isn't a type. Can't name a variable object, so objectt it is. 
    public string objectt { get; set; }
    public string subject { get; set; }
    public string possessive { get; set; }
    public string inPossessive { get; set; }
    public string reflexive { get; set; }
    public Conjugation conjugation { get; set; }


    public Pronoun(string objectt, string subject, string possessive, string inPossessive, string reflexive,
        Conjugation conjugation)
    {
        this.objectt = objectt;
        this.subject = subject;
        this.possessive = possessive;
        this.inPossessive = inPossessive;
        this.reflexive = reflexive;
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
                return objectt;
            case "subject":
                return subject;
            case "poss":
                return possessive;
            case "inposs":
                return inPossessive;
            case "self":
                return reflexive;
            default:
                return $"error--\"{type}\"";
        }
    }
}
