using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
///     Extensions to turn <see cref="byte" />s into <see cref="MySqlParameter" />s
/// </summary>
public static class ByteSqlParameterExtensions
{
    /// <summary>
    ///     Converts a byte to a <see cref="MySqlParameter" />
    /// </summary>
    /// <param name="Byte"></param>
    /// <param name="Name"></param>
    /// <param name="DbType"><see cref="MySqlDbType.Byte" /> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this byte Byte, string Name,
        MySqlDbType DbType = MySqlDbType.Byte)
    {
        return new MySqlParameter(Name, DbType) { Value = Byte };
    }

    /// <summary>
    ///     Converts a byte to a <see cref="MySqlParameter" />
    /// </summary>
    /// <param name="Byte"></param>
    /// <param name="Name"></param>
    /// <param name="DbType"><see cref="MySqlDbType.Byte" /> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this byte? Byte, string Name,
        MySqlDbType DbType = MySqlDbType.Byte)
    {
        return new MySqlParameter(Name, DbType) { Value = Byte ?? (object)DBNull.Value };
    }

    /// <summary>
    ///     Converts a byte array to a <see cref="MySqlParameter" />
    /// </summary>
    /// <param name="Byte"></param>
    /// <param name="Name"></param>
    /// <param name="Size">Pass -1 for "max"-size SQL types (e.g. varbinary(max))</param>
    /// <param name="DbType"><see cref="MySqlDbType.VarBinary" /> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this byte[]? Byte, string Name, int Size,
        MySqlDbType DbType = MySqlDbType.VarBinary)
    {
        return new MySqlParameter(Name, DbType, Size) { Value = Byte ?? (object)DBNull.Value };
    }
}