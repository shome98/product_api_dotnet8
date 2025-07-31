using Dapper;
using product_api_dotnet8.Db;
using System.Data;
using System.Threading.Tasks;

public class DataContextPostgres
{
    private readonly DapperContextPostgres _dapperContext;

    public DataContextPostgres(DapperContextPostgres dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<IEnumerable<Item>> GetAllItemsAsync()
    {
        const string sql = "SELECT * FROM \"Items\"";
        using var connection = await _dapperContext.CreateConnectionAsync();
        return await connection.QueryAsync<Item>(sql);
    }

    public async Task<Item> GetItemByIdAsync(int id)
    {
        var sql = "SELECT * FROM \"Items\" WHERE \"Id\" = @Id";
        using var connection = await _dapperContext.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<Item>(sql, new { Id = id });
    }
    public async Task<bool> CreateItemAsync(Item item)
    {
        const string sql = "INSERT INTO \"Items\" (\"Name\", \"Description\") VALUES (@Name, @Description)";
        using var connection = await _dapperContext.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(sql, item);
        return result > 0;
    }

    public async Task<bool> UpdateItemAsync(Item item)
    {
        const string sql = "UPDATE \"Items\" SET \"Name\" = @Name, \"Description\" = @Description WHERE Id = @Id";
        using var connection = await _dapperContext.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(sql, item);
        return result > 0;
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        const string sql = "DELETE FROM \"Items\" WHERE \"Id\" = @Id";
        using var connection = await _dapperContext.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}