using System.ComponentModel.DataAnnotations;

namespace MyReptileFamilyAPI.CORE.Models;

public class ReptileSearchable
{
    public string Category { get; set; } = "";

    /// <summary>
    /// What species are you looking for. Leave blank to search for all.
    /// </summary>
    public string Species { get; set; } = "";

    /// <summary>
    /// Only working if a species is defined. What morph are you searching for. Leave blank to search for all
    /// </summary>
    public string Morph { get; set; } = "";

    /// <summary>
    /// Price range for the reptile you are looking to buy.
    /// </summary>
    public Range Price { get; set; } = new Range(new Index(0), new Index(int.MaxValue));

    /// <summary>
    /// Does the listing have images of the animal species.
    /// </summary>
    public bool HasImage { get; set; } = false;

    /// <summary>
    /// What rating would you like the seller to have. rating is from 0-5 what ever number given it will search for that number and higher.
    /// </summary>
    [Range(0, 5)] public int SellerRating { get; set; } = 0;

    /// <summary>
    /// How many generations does this animal have on MyReptileFamily
    /// </summary>
    [Range(0, 10)] public int Generations { get; set; } = 0;
}