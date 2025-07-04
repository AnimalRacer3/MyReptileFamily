﻿using MyReptileFamilyLibrary.Records;
using MyReptileFamilyLibrary.SQL.Abstractions;
using MySqlConnector;

namespace MyReptileFamilyAPI.SQL;

public class GetBadWordsQuery : IDapperQuery<string>
{
    public string SQL => """
                         SELECT
                            BadWords 
                         FROM BadWords
                         """;
    public DapperParameter DapperParameter => new();
}