using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
/// Extensions to turn <see cref="List{T}"/>s into <see cref="MySqlParameter"/>s
/// </summary>
public static class ListSqlParameterExtensions
{
    /// <summary>
    /// Generates "ColumnName IN (ParameterName0, ParameterName1)" part of SQL operation.
    /// When needing <see cref="MySqlParameter"/>s, call "_yourList.ToSqlParametersForWhereInClause()" />
    /// </summary>
    /// <param name="_p_Values">
    /// The list of values that will be added as <see cref="MySqlParameter"/>s
    /// </param>
    /// <param name="_p_ColumnName"> The name of the SQL table column being checked against </param>
    /// <param name="_p_ParameterName">The name of the SQL parameter. Include the '@' at the beginning!</param>
    /// <returns>String to add to the SQL operation</returns>
    /// <exception cref="ArgumentException">Thrown when given empty list (at least one is required)</exception>
    public static string GetInClauseForList<T>(this List<T> _p_Values, string _p_ColumnName, string _p_ParameterName)
    {
        if (_p_Values.Count == 0) throw new ArgumentException("Must provide at least one value", nameof(_p_Values));

        var _parameters = _p_Values.Select((_, _p_Index) => $"{_p_ParameterName}{_p_Index}");
        return $"{_p_ColumnName} IN ({string.Join(", ", _parameters)})";
    }

    internal static List<MySqlParameter> ToSqlParametersForWhereInClauseInternal<T>(this List<T> _p_Values, Func<T, MySqlParameter> _p_GetSqlParameterFunc)
    {
        if (_p_Values.Count == 0) throw new ArgumentException("Must provide at least one value", nameof(_p_Values));

        return _p_Values
            .Select((_p_Value, _p_Index) =>
            {
                MySqlParameter _sqlParameter = _p_GetSqlParameterFunc(_p_Value);
                _sqlParameter.ParameterName += _p_Index;
                return _sqlParameter;
            })
            .ToList();
    }
}
