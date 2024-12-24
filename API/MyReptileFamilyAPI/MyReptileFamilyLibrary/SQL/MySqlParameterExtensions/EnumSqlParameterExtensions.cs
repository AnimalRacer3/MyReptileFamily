using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
///     Extensions to turn <see cref="Enum" />s into <see cref="MySqlParameter" />s
/// </summary>
public static class EnumSqlParameterExtensions
{
    /// <summary>
    ///     Converts an enum to a <see cref="MySqlParameter" />
    /// </summary>
    /// <param name="Enum">The char to turn into a parameter (nulls will become <see cref="DBNull.Value" />)</param>
    /// <param name="Name"></param>
    /// <param name="DbType"><see cref="MySqlDbType.Enum" /> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this Enum? Enum, string Name,
        MySqlDbType DbType = MySqlDbType.Enum)
    {
        return new MySqlParameter(Name, DbType) { Value = Enum ?? (object)DBNull.Value };
    }
}