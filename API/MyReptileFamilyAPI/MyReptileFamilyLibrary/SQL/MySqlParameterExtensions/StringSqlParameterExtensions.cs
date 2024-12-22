using MySqlConnector;
using System.Data;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
/// Extensions to turn <see cref="string"/>s into <see cref="MySqlParameter"/>s
/// </summary>
public static class StringSqlParameterExtensions
{
    /// <summary>
    /// Converts a string to SQL Parameter
    /// </summary>
    /// <param name="_p_String">The string to turn into a parameter (nulls will become <see cref="DBNull.Value"/>)</param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_Size">Pass -1 for "max"-size SQL types (e.g. varchar(max))</param>
    /// <param name="_p_DbType"></param>
    /// <param name="_p_TreatNullOrWhiteSpaceAsNull">
    ///     When true, null/whitespace-only strings will become <see cref="DBNull.Value"/>.
    ///     When false, they will become an empty string.
    ///     If <paramref name="_p_TreatNullAsNull"/> is true, nulls will become <see cref="DBNull.Value"/>.
    /// </param>
    /// <param name="_p_TreatNullAsNull">
    ///     When true, null strings will become <see cref="DBNull.Value"/>.
    ///     When false, they will become an empty string.
    /// </param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this string? _p_String, string _p_Name, int _p_Size, MySqlDbType _p_DbType = MySqlDbType.VarChar,
        bool _p_TreatNullOrWhiteSpaceAsNull = false, bool _p_TreatNullAsNull = false)
    {
        object _value;
        if (_p_String is null && _p_TreatNullAsNull)
        {
            _value = DBNull.Value;
        }
        else if (string.IsNullOrWhiteSpace(_p_String) && _p_TreatNullOrWhiteSpaceAsNull)
        {
            _value = DBNull.Value;
        }
        else
        {
            _value = !string.IsNullOrWhiteSpace(_p_String) ? _p_String : string.Empty;
        }
        return new(_p_Name, _p_DbType, _p_Size) { Value = _value };
    }

    /// <summary>
    /// Provides a list of SQL Parameters with indexed names, one for each item in the list.
    /// Example: @Name0, @Name1
    /// When building the SQL operation, use <see cref="ArgumentException"/> to add
    /// "IN (@Name0, @Name1)" for you automatically.
    /// </summary>
    /// <returns>A comma-separated list of values, even when an empty list is given</returns>
    /// <exception cref="ListSqlParameterExtensions">Thrown when given empty list (at least one is required)</exception>
    public static List<MySqlParameter> ToSqlParametersForWhereInClause(this List<string> _p_Values, string _p_Name, int _p_Size, MySqlDbType _p_DbType = MySqlDbType.VarChar, bool _p_TreatNullOrWhiteSpaceAsNull = false, bool _p_TreatNullAsNull = false)
        => _p_Values.ToSqlParametersForWhereInClauseInternal(_p_String => _p_String.ToSqlParameter(_p_Name, _p_Size, _p_DbType, _p_TreatNullOrWhiteSpaceAsNull, _p_TreatNullAsNull));

    /// <summary>
    /// Provides a list of SQL Parameters with indexed names, one for each item in the list.
    /// Example: @Name0, @Name1
    /// When building the SQL operation, use <see cref="ListSqlParameterExtensions.GetInClauseForList{T}(List{T},string,string)"/> to add
    /// "IN (@Name0, @Name1)" for you automatically.
    /// </summary>
    /// <returns>A comma-separated list of values, even when an empty list is given</returns>
    /// <exception cref="ArgumentException">Thrown when given empty list (at least one is required)</exception>
    public static List<MySqlParameter> ToSqlParametersForWhereInClauseNullable(this List<string?> _p_Values, string _p_Name, int _p_Size, MySqlDbType _p_DbType = MySqlDbType.VarChar, bool _p_TreatNullOrWhiteSpaceAsNull = false, bool _p_TreatNullAsNull = false)
        => _p_Values.ToSqlParametersForWhereInClauseInternal(_p_String => _p_String.ToSqlParameter(_p_Name, _p_Size, _p_DbType, _p_TreatNullOrWhiteSpaceAsNull, _p_TreatNullAsNull));
}
