using System.ComponentModel.DataAnnotations;

namespace MyReptileFamilyAPI.AppSettings;

public class DbSettings
{
    [Required] public string Server = "";
    [Required] public string Database = "";
    [Required] public string Username = "";
    [Required] public string Password = "";

    public string ConnectionString => $"Server={Server};Database={Database};User={Username};Password={Password}";
}