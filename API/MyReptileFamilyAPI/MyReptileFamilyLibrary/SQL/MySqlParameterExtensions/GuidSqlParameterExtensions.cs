using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
///     Extensions to turn <see cref="Guid" />s into <see cref="MySqlParameter" />s
/// </summary>
public static class GuidSqlParameterExtensions
{
    /// <summary>
    ///     Converts a GUID to a SQL Parameter
    /// </summary>
    /// <param name="Guid"></param>
    /// <param name="Name"></param>
    /// <param name="DbType"></param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this Guid Guid, string Name,
        MySqlDbType DbType = MySqlDbType.Guid)
    {
        return new MySqlParameter(Name, DbType) { Value = Guid };
    }
}