using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
///     Extensions to turn <see cref="short" />s into <see cref="MySqlParameter" />s
/// </summary>
public static class ShortSqlParameterExtensions
{
    /// <summary>
    ///     Converts a short to a <see cref="MySqlParameter" />
    /// </summary>
    /// <param name="Short"></param>
    /// <param name="Name"></param>
    /// <param name="DbType"><see cref="MySqlDbType.Int16" /> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this short Short, string Name,
        MySqlDbType DbType = MySqlDbType.Int16)
    {
        return new MySqlParameter(Name, DbType) { Value = Short };
    }

    /// <summary>
    ///     Converts a short to a <see cref="MySqlParameter" />
    /// </summary>
    /// <param name="Short"></param>
    /// <param name="Name"></param>
    /// <param name="DbType"><see cref="MySqlDbType.Int16" /> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this short? Short, string Name,
        MySqlDbType DbType = MySqlDbType.Int16)
    {
        return new MySqlParameter(Name, DbType) { Value = Short ?? (object)DBNull.Value };
    }
}