using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
///     Extensions to turn <see cref="int" />s into <see cref="MySqlParameter" />s
/// </summary>
public static class IntSqlParameterExtensions
{
    /// <summary>
    ///     Converts an int to a <see cref="MySqlParameter" />
    /// </summary>
    /// <param name="Int"></param>
    /// <param name="Name"></param>
    /// <param name="DbType"><see cref="MySqlDbType.Int" /> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this int Int, string Name,
        MySqlDbType DbType = MySqlDbType.Int32)
    {
        return new MySqlParameter(Name, DbType) { Value = Int };
    }

    /// <summary>
    ///     Converts an int to a <see cref="MySqlParameter" />
    /// </summary>
    /// <param name="Int"></param>
    /// <param name="Name"></param>
    /// <param name="DbType"><see cref="MySqlDbType.Int" /> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this int? Int, string Name,
        MySqlDbType DbType = MySqlDbType.Int32)
    {
        return new MySqlParameter(Name, DbType) { Value = Int ?? (object)DBNull.Value };
    }
}