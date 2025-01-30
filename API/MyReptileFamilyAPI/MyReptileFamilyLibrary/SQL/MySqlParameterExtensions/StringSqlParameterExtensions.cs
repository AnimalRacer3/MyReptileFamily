using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
///     Extensions to turn <see cref="string" />s into <see cref="MySqlParameter" />s
/// </summary>
public static class StringSqlParameterExtensions
{
    /// <summary>
    ///     Converts a string to SQL Parameter VarChar
    /// </summary>
    /// <param name="String">The string to turn into a parameter (nulls will become <see cref="DBNull.Value" />)</param>
    /// <param name="Name"></param>
    /// <param name="Size">Pass -1 for "max"-size SQL types (e.g. varchar(max))</param>
    /// <param name="DbType"></param>
    /// <param name="TreatNullOrWhiteSpaceAsNull">
    ///     When true, null/whitespace-only strings will become <see cref="DBNull.Value" />.
    ///     When false, they will become an empty string.
    ///     If <paramref name="TreatNullAsNull" /> is true, nulls will become <see cref="DBNull.Value" />.
    /// </param>
    /// <param name="TreatNullAsNull">
    ///     When true, null strings will become <see cref="DBNull.Value" />.
    ///     When false, they will become an empty string.
    /// </param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this string? String, string Name, int Size,
        MySqlDbType DbType = MySqlDbType.VarChar,
        bool TreatNullOrWhiteSpaceAsNull = false, bool TreatNullAsNull = false)
    {
        object _value;
        if (String is null && TreatNullAsNull)
            _value = DBNull.Value;
        else if (string.IsNullOrWhiteSpace(String) && TreatNullOrWhiteSpaceAsNull)
            _value = DBNull.Value;
        else
            _value = !string.IsNullOrWhiteSpace(String) ? String : string.Empty;
        return new MySqlParameter(Name, DbType, Size) { Value = _value };
    }

    /// <summary>
    ///     Converts a string to SQL Parameter Text
    /// </summary>
    /// <param name="String">The string to turn into a parameter (nulls will become <see cref="DBNull.Value" />)</param>
    /// <param name="Name"></param>
    /// <param name="DbType"></param>
    /// <param name="TreatNullOrWhiteSpaceAsNull">
    ///     When true, null/whitespace-only strings will become <see cref="DBNull.Value" />.
    ///     When false, they will become an empty string.
    ///     If <paramref name="TreatNullAsNull" /> is true, nulls will become <see cref="DBNull.Value" />.
    /// </param>
    /// <param name="TreatNullAsNull">
    ///     When true, null strings will become <see cref="DBNull.Value" />.
    ///     When false, they will become an empty string.
    /// </param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this string? String, string Name, MySqlDbType DbType = MySqlDbType.Text,
        bool TreatNullOrWhiteSpaceAsNull = false, bool TreatNullAsNull = false)
    {
        object _value;
        if (String is null && TreatNullAsNull)
            _value = DBNull.Value;
        else if (string.IsNullOrWhiteSpace(String) && TreatNullOrWhiteSpaceAsNull)
            _value = DBNull.Value;
        else
            _value = !string.IsNullOrWhiteSpace(String) ? String : string.Empty;
        return new MySqlParameter(Name, DbType) { Value = _value };
    }

    /// <summary>
    ///     Provides a list of SQL Parameters with indexed names, one for each item in the list.
    ///     Example: @Name0, @Name1
    ///     When building the SQL operation, use <see cref="ArgumentException" /> to add
    ///     "IN (@Name0, @Name1)" for you automatically.
    /// </summary>
    /// <returns>A comma-separated list of values, even when an empty list is given</returns>
    /// <exception cref="ListSqlParameterExtensions">Thrown when given empty list (at least one is required)</exception>
    public static List<MySqlParameter> ToSqlParametersForWhereInClause(this List<string> Values, string Name,
        int Size, MySqlDbType DbType = MySqlDbType.VarChar, bool TreatNullOrWhiteSpaceAsNull = false,
        bool TreatNullAsNull = false)
    {
        return Values.ToSqlParametersForWhereInClauseInternal(String =>
            String.ToSqlParameter(Name, Size, DbType, TreatNullOrWhiteSpaceAsNull, TreatNullAsNull));
    }

    /// <summary>
    ///     Provides a list of SQL Parameters with indexed names, one for each item in the list.
    ///     Example: @Name0, @Name1
    ///     When building the SQL operation, use
    ///     <see cref="ListSqlParameterExtensions.GetInClauseForList{T}(List{T},string,string)" /> to add
    ///     "IN (@Name0, @Name1)" for you automatically.
    /// </summary>
    /// <returns>A comma-separated list of values, even when an empty list is given</returns>
    /// <exception cref="ArgumentException">Thrown when given empty list (at least one is required)</exception>
    public static List<MySqlParameter> ToSqlParametersForWhereInClauseNullable(this List<string?> Values,
        string Name, int Size, MySqlDbType DbType = MySqlDbType.VarChar,
        bool TreatNullOrWhiteSpaceAsNull = false, bool TreatNullAsNull = false)
    {
        return Values.ToSqlParametersForWhereInClauseInternal(String =>
            String.ToSqlParameter(Name, Size, DbType, TreatNullOrWhiteSpaceAsNull, TreatNullAsNull));
    }
}