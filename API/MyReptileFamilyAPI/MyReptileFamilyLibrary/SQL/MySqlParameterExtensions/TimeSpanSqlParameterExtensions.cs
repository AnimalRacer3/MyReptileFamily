using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
///     Extensions to turn <see cref="TimeSpan" />s into <see cref="MySqlParameter" />s
/// </summary>
public static class TimeSpanSqlParameterExtensions
{
    /// <summary>
    ///     Converts a TimeSpan object to a SqlParameter.
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="Name"></param>
    /// <param name="DbType"></param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this TimeSpan Value, string Name,
        MySqlDbType DbType = MySqlDbType.Time)
    {
        return new MySqlParameter(Name, DbType) { Value = Value };
    }

    /// <summary>
    ///     Converts a Nullable TimeSpan object to a SqlParameter.  Null values will be inserted with DBNull.Value.
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="Name"></param>
    /// <param name="DbType"></param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this TimeSpan? Value, string Name,
        MySqlDbType DbType = MySqlDbType.Time)
    {
        return new MySqlParameter(Name, DbType) { Value = Value.HasValue ? Value.Value : DBNull.Value };
    }
}