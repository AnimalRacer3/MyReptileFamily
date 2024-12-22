using System.ComponentModel.DataAnnotations;
using MyReptileFamilyLibrary.AppSettings;

namespace MyReptileFamilyAPI.AppSettings;

public class DbSettings : IMySQLConnectionString
{
    [Required] public string Server { get; set; } = "";
    public int Port { get; set; } = 3306;
    [Required] public string Database { get; set; } = "";
    [Required] public string Username { get; set; } = "";
    [Required] public string Password { get; set; } = "";

    public string MySQLConnectionString => $"Server={Server};Port={Port};Uid={Username};pwd=\"{Password}\";Database={Database}";
}