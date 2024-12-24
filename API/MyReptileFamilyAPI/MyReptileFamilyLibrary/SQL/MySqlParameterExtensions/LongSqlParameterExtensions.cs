using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
///     Extensions to turn <see cref="long" />s into <see cref="MySqlParameter" />s
/// </summary>
public static class LongSqlParameterExtensions
{
    /// <summary>
    ///     Converts a long (Int64) to a <see cref="MySqlParameter" />
    /// </summary>
    /// <param name="Long"></param>
    /// <param name="Name"></param>
    /// <param name="DbType"><see cref="MySqlDbType.Int64" /> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this long Long, string Name,
        MySqlDbType DbType = MySqlDbType.Int64)
    {
        return new MySqlParameter(Name, DbType) { Value = Long };
    }

    /// <summary>
    ///     Converts a long (Int64) to a <see cref="MySqlParameter" />
    /// </summary>
    /// <param name="Long"></param>
    /// <param name="Name"></param>
    /// <param name="DbType"><see cref="MySqlDbType.Int64" /> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this long? Long, string Name,
        MySqlDbType DbType = MySqlDbType.Int64)
    {
        return new MySqlParameter(Name, DbType) { Value = Long ?? (object)DBNull.Value };
    }
}