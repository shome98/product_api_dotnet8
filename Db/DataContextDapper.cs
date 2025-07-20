using System;
using System.Data;
using Dapper;
using MySqlConnector;

namespace product_api_dotnet8.Db;

// public class DataContextDapper
// {
//     private readonly IConfiguration _config;
//     public DataContextDapper(IConfiguration config)
//     {
//         _config = config;
//     }

//     public IEnumerable<T> LoadData<T>(string sql)
//     {
//         IDbConnection dbConnection = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
//         return dbConnection.Query<T>(sql);
//     }
//     public T InsertData<T>(string sql)
//     {
//         IDbConnection dbConnection = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
//         return dbConnection.Query<T>(sql);
//     }
// }
using Dapper;
using System.Data;
using System.Threading.Tasks;

public class DataContextDapper
{
    private readonly DapperContext _dapperContext;

    public DataContextDapper(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<IEnumerable<Item>> GetAllItemsAsync()
    {
        var sql = "SELECT * FROM Items";
        using (var connection = await _dapperContext.CreateConnectionAsync())
        {
            return await connection.QueryAsync<Item>(sql);
        }
    }

    public async Task<Item> GetItemByIdAsync(int id)
    {
        var sql = "SELECT * FROM Items WHERE Id = @Id";
        using (var connection = await _dapperContext.CreateConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<Item>(sql, new { Id = id });
        }
    }

    public async Task<bool> CreateItemAsync(Item item)
    {
        var sql = "INSERT INTO Items (Name, Description) VALUES (@Name, @Description)";
        using (var connection = await _dapperContext.CreateConnectionAsync())
        {
            var result = await connection.ExecuteAsync(sql, item);
            return result > 0;
        }
    }

    public async Task<bool> UpdateItemAsync(Item item)
    {
        var sql = "UPDATE Items SET Name = @Name, Description = @Description WHERE Id = @Id";
        using (var connection = await _dapperContext.CreateConnectionAsync())
        {
            var result = await connection.ExecuteAsync(sql, item);
            return result > 0;
        }
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        var sql = "DELETE FROM Items WHERE Id = @Id";
        using (var connection = await _dapperContext.CreateConnectionAsync())
        {
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
    }
}