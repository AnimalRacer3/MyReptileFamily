using System.ComponentModel.DataAnnotations;
using MyReptileFamilyLibrary.AppSettings;
using MyReptileFamilyLibrary.Records;

namespace MyReptileFamilyAPI.AppSettings;

public class EmailSettings : ISendGridSettings
{
    [Required] public string SendGridApiKey { get; set; } = "";
    [Required] public Email BaseRegisterEmail { get; set; } = new Email("", "", "");
    public bool EmailEnabled { get; set; } = true;
    public bool LogEmails { get; set; } = false;
}