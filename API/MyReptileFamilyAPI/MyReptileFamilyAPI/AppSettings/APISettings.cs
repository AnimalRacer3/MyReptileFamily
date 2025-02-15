using System.ComponentModel.DataAnnotations;

namespace MyReptileFamilyAPI.AppSettings;

public class APISettings
{
    [Required] public string URL { get; set; } = "";
    [Required] public int Port { get; set; }
}