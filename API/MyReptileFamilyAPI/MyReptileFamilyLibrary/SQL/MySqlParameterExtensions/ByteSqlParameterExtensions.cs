using System.Data;
using MySql.Data.MySqlClient;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
/// Extensions to turn <see cref="byte"/>s into <see cref="MySqlParameter"/>s
/// </summary>
public static class ByteSqlParameterExtensions
{
    /// <summary>
    /// Converts a byte to a <see cref="MySqlParameter"/>
    /// </summary>
    /// <param name="_p_Byte"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"><see cref="MySqlDbType.Byte"/> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this byte _p_Byte, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Byte)
        => new(_p_Name, _p_DbType) { Value = _p_Byte };

    /// <summary>
    /// Converts a byte to a <see cref="MySqlParameter"/>
    /// </summary>
    /// <param name="_p_Byte"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"><see cref="MySqlDbType.Byte"/> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this byte? _p_Byte, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Byte)
        => new(_p_Name, _p_DbType) { Value = _p_Byte ?? (object) DBNull.Value };

    /// <summary>
    /// Converts a byte array to a <see cref="MySqlParameter"/>
    /// </summary>
    /// <param name="_p_Byte"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_Size">Pass -1 for "max"-size SQL types (e.g. varbinary(max))</param>
    /// <param name="_p_DbType"><see cref="MySqlDbType.VarBinary"/> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this byte[]? _p_Byte, string _p_Name, int _p_Size, MySqlDbType _p_DbType = MySqlDbType.VarBinary)
        => new(_p_Name, _p_DbType, _p_Size) { Value = _p_Byte ?? (object) DBNull.Value };
}
