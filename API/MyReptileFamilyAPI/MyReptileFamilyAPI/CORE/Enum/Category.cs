using MyReptileFamilyAPI.CORE.Attributes;

namespace MyReptileFamilyAPI.CORE.Enum;

public enum Category
{
    [StringValue("All")] All,
    [StringValue("Snakes")] Snakes,
    [StringValue("Lizards")] Lizards,
    [StringValue("Geckos")] Geckos,
    [StringValue("Tortoises")] Tortoises,
    [StringValue("Turtles")] Turtles,
    [StringValue("Amphibians")] Amphibians,
    [StringValue("Invertebrates")] Invertebrates
}