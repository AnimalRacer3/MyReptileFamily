using MySql.Data.MySqlClient;
using System.Data;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
/// General extension methods for <see cref="MySqlParameter" />
/// </summary>
public static class SqlParameterBuildExtensions
{
    /// <summary>
    /// Uses <see cref="MySqlParameter"/>s to build an SQL INSERT statement for <paramref name="_p_TableName"/>
    /// </summary>
    public static string BuildInsertSQL(this List<MySqlParameter> _p_Parameters, string _p_TableName)
    {
        string _columns = string.Join($"{Environment.NewLine}    , ", _p_Parameters.Select(_p_Parameter => _p_Parameter.ParameterName[1..]));
        string _values = string.Join($"{Environment.NewLine}    , ", _p_Parameters.Select(_p_Parameter => _p_Parameter.ParameterName));
        return $"""
                INSERT INTO {_p_TableName}
                (
                      {_columns}
                )
                VALUES
                (
                      {_values}
                );
                """;
    }

    /// <summary>
    /// Returns a single string containing every input parameter's name and value.
    /// </summary>
    public static string BuildInputStringList(this List<MySqlParameter> _p_Parameters)
    {
        List<ParameterDirection> _parametersToList = [ParameterDirection.Input, ParameterDirection.InputOutput];
        return string.Join(", ", _p_Parameters
            .Where(_p_Parameter => _parametersToList.Contains(_p_Parameter.Direction))
            .Select(_p_Parameter => $"{_p_Parameter.ParameterName} [{_p_Parameter.Value}]"));
    }
}
