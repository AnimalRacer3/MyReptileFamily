using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL;

/// <inheritdoc />
public class MySQLConnectionWrapper(MySqlConnection SQLConnection) : IMySQLConnection
{
    public MySqlConnection SQLConn => SQLConnection;

    /// <inheritdoc />
    public Task OpenAsync(CancellationToken CancellationToken)
    {
        return SQLConn.OpenAsync(CancellationToken);
    }

    /// <inheritdoc />
    public ValueTask<MySqlTransaction> BeginDbTransactionAsync()
    {
        return SQLConn.BeginTransactionAsync();
    }

    /// <inheritdoc />
    public Task CloseAsync()
    {
        return SQLConn.CloseAsync();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        SQLConn.Dispose();
    }

    /// <inheritdoc />
    public IDbTransaction BeginTransaction()
    {
        return SQLConn.BeginTransaction();
    }

    /// <inheritdoc />
    public IDbTransaction BeginTransaction(IsolationLevel IsolationLevel)
    {
        return SQLConn.BeginTransaction(IsolationLevel);
    }

    /// <inheritdoc />
    public void ChangeDatabase(string DatabaseName)
    {
        SQLConn.ChangeDatabase(DatabaseName);
    }

    /// <inheritdoc />
    public void Close()
    {
        SQLConn.Close();
    }

    /// <inheritdoc />
    public IDbCommand CreateCommand()
    {
        return SQLConn.CreateCommand();
    }

    /// <inheritdoc />
    public void Open()
    {
        SQLConn.Open();
    }

    /// <inheritdoc />
    [AllowNull]
    public string ConnectionString
    {
        get => SQLConn.ConnectionString;
        set => SQLConn.ConnectionString = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <inheritdoc />
    public int ConnectionTimeout => SQLConn.ConnectionTimeout;

    /// <inheritdoc />
    public string Database => SQLConn.Database;

    /// <inheritdoc />
    public ConnectionState State => SQLConn.State;

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        return SQLConn.DisposeAsync();
    }

    /// <inheritdoc />
    public ISite? Site
    {
        get => SQLConn.Site;
        set => SQLConn.Site = value;
    }

    /// <inheritdoc />
    public event EventHandler? Disposed
    {
        add => SQLConn.Disposed += value;
        remove => SQLConn.Disposed -= value;
    }
}