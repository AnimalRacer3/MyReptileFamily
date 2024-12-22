using System.ComponentModel;
using System.Data;
using System.Data.Common;
using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL;

/// <inheritdoc cref="MySqlConnection" />
public interface IMySQLConnection : IDbConnection, IAsyncDisposable, IComponent
{
    /// <inheritdoc cref="MySqlConnection.OpenAsync(CancellationToken)" />
    Task OpenAsync(CancellationToken _p_CancellationToken);

    /// <inheritdoc cref="DbConnection.BeginDbTransactionAsync" />
    ValueTask<MySqlTransaction> BeginDbTransactionAsync();

    /// <inheritdoc cref="MySqlConnection.CloseAsync()" />
    Task CloseAsync();
}