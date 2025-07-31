using System;
using System.Data;
using Npgsql;

namespace product_api_dotnet8.Db;

public class DapperContextPostgres
{
    private readonly string _connectionString;

    public DapperContextPostgres(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}
