using MyReptileFamilyLibrary.SQL;

namespace MyReptileFamilyLibrary.AppSettings;

/// <summary>
///     Add to settings files that have a SQL connection string.
///     If you need a second connection string for the EDI database, add <see cref="IMySQLConnectionString"/> to
///     your settings.
///     This connection is automatically used when creating SQL connections in <see cref="IMRFRepository.CreateMySQLConnection"/>
/// </summary>
public interface IMySQLConnectionString
{
    /// <summary>
    ///     Used to open connections to a SQL database
    /// </summary>
    string MySQLConnectionString { get; }
}