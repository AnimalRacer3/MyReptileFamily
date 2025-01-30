using System.ComponentModel.DataAnnotations;

namespace MyReptileFamilyAPI.AppSettings;

public class APISettings
{
    [Required] public string URL { get; set; } = "";
}