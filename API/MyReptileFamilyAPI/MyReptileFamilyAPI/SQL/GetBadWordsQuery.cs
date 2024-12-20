using MyReptileFamilyLibrary.SQL.Abstractions;
using MySql.Data.MySqlClient;

namespace MyReptileFamilyAPI.SQL;

public class GetBadWordsQuery : IDapperQuery<string>
{
    public string SQL => """
                         SELECT
                            BadWords 
                         FROM BadWords
                         """;
    public MySqlParameter[] Parameters => [];
}