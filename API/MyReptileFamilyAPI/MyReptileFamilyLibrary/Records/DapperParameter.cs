using MySql.Data.MySqlClient;

namespace MyReptileFamilyLibrary.Records;

public record DapperParameter
{
    public DapperParameter(List<MySqlParameter> Parameters)
    {
        this.Parameters = Parameters;
    }
    public IEnumerable<MySqlParameter> Parameters { get; private set; }
    
}