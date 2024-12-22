using MyReptileFamilyLibrary.Records;
using MyReptileFamilyLibrary.SQL.Abstractions;
using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL;

public class CreateTemporaryTable(string TempTableName, MySqlDbType SqlType, int? Size, int? Precision, int? Scale, List<string>? EnumList) : IDapperSQL
{
    public string SQL => $"CREATE TEMPORARY TABLE {TempTableName} (ID INT, Item {MapMySqlDbTypeToSqlType(SqlType)}{(Size != null ? $"({Size})" : (Precision != null && Scale != null) ? $"({Precision},{Scale})" : EnumList != null ? $"({string.Join(",",EnumList)})" : "")});";
    public DapperParameter DapperParameter => new();

    private static string MapMySqlDbTypeToSqlType(MySqlDbType sqlType)
    {
        return sqlType switch
        {
            MySqlDbType.Bit => "BIT",
            MySqlDbType.Byte => "TINYINT",
            MySqlDbType.VarChar => "VARCHAR",
            MySqlDbType.Date => "DATE",
            MySqlDbType.DateTime => "DATETIME",
            MySqlDbType.Decimal => "DECIMAL",
            MySqlDbType.Guid => "VARCHAR(36)",
            MySqlDbType.Int32 => "INT",
            MySqlDbType.Int64 => "BIGINT",
            MySqlDbType.Int16 => "SMALLINT",
            MySqlDbType.Time => "TIME",
            MySqlDbType.UByte => "TINYINT UNSIGNED",
            MySqlDbType.Text => "TEXT",
            MySqlDbType.Float => "FLOAT",
            MySqlDbType.Double => "DOUBLE",
            MySqlDbType.Blob => "BLOB",
            MySqlDbType.JSON => "JSON",
            MySqlDbType.Enum => "ENUM",
            _ => throw new ArgumentException($"Unsupported MySqlDbType: {sqlType}")
        };
    }
}