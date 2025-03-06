using MySqlConnector;

namespace MyReptileFamilyLibrary.Records;

public record DapperParameter
{
    public DapperParameter()
    {
        Parameters = new Dictionary<string, object?>();
    }

    public DapperParameter(MySqlParameter MySqlParameter)
    {
        Parameters = ((List<MySqlParameter>) [MySqlParameter]).ToDictionary(param => param.ParameterName,
            param => param.Value == DBNull.Value ? null : param.Value);
    }

    public DapperParameter(IEnumerable<MySqlParameter> MySqlParameters)
    {
        Parameters = MySqlParameters.ToDictionary(param => param.ParameterName,
            param => param.Value == DBNull.Value ? null : param.Value);
    }

    public Dictionary<string, object?> Parameters { get; init; }
}