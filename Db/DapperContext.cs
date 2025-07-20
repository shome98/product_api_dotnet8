using Microsoft.Data.SqlClient;
using System.Data;
using MySqlConnector;

namespace product_api_dotnet8.Db;

public class DapperContext
{
    private readonly string? _connectionString;

    public DapperContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}
