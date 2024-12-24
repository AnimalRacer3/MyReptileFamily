using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
///     Extensions to turn <see cref="bool" />s into <see cref="MySqlParameter" />s
/// </summary>
public static class BoolSqlParameterExtensions
{
    /// <summary>
    ///     Converts a bool to a <see cref="MySqlParameter" />
    /// </summary>
    /// <param name="Bool"></param>
    /// <param name="Name"></param>
    /// <param name="DbType"><see cref="MySqlDbType.Bit" /> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this bool Bool, string Name,
        MySqlDbType DbType = MySqlDbType.Bit)
    {
        return new MySqlParameter(Name, DbType) { Value = Bool };
    }

    /// <summary>
    ///     Converts a bool to a <see cref="MySqlParameter" />
    /// </summary>
    /// <param name="Bool"></param>
    /// <param name="Name"></param>
    /// <param name="DbType"><see cref="MySqlDbType.Bit" /> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this bool? Bool, string Name,
        MySqlDbType DbType = MySqlDbType.Bit)
    {
        return new MySqlParameter(Name, DbType) { Value = Bool ?? (object)DBNull.Value };
    }
}