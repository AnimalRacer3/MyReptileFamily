using System.ComponentModel.DataAnnotations;

namespace MyReptileFamilyAPI.AppSettings;

public class APISettings
{
    [Required] public string URL { get; set; } = "";
    [Required] public int Port { get; set; }
    [Required] public string PathToCert { get; set; } = "";
    [Required] public string CertPassword { get; set; } = "";
}