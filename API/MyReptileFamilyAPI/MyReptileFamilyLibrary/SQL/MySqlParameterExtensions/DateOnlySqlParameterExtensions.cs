using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
///     Extensions to turn <see cref="DateOnly" />s into <see cref="MySqlParameter" />s
/// </summary>
public static class DateOnlySqlParameterExtensions
{
    /// <summary>
    ///     Converts a DateOnly object to a SqlParameter
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="Name"></param>
    /// <param name="DbType"></param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this DateOnly Value, string Name,
        MySqlDbType DbType = MySqlDbType.Date)
    {
        return new MySqlParameter(Name, DbType) { Value = Value };
    }

    /// <summary>
    ///     Converts a Nullable DateOnly object to a SqlParameter.  Null values will be inserted with DBNull.Value.
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="Name"></param>
    /// <param name="DbType"></param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this DateOnly? Value, string Name,
        MySqlDbType DbType = MySqlDbType.Date)
    {
        return new MySqlParameter(Name, DbType) { Value = Value.HasValue ? Value.Value : DBNull.Value };
    }
}