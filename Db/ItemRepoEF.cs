using System;
using Microsoft.EntityFrameworkCore;

namespace product_api_dotnet8.Db;

public class ItemRepoEF
{
    private readonly DataContextEFPostgres _context;
    public ItemRepoEF(DataContextEFPostgres conetxt)
    {
        _context = conetxt;
    }

    public async Task<IEnumerable<Item>> GetAllItemsAsync()
    {
        const string sql = "SELECT * FROM \"Items\"";
        return await _context.Items.FromSqlRaw(sql).ToListAsync();
    }

    public async Task<Item> GetItemByIdAsync(int id)
    {
        var sql = "SELECT * FROM \"Items\" WHERE \"Id\" = @Id";
        return await _context.Items.FromSqlRaw(sql).FirstOrDefaultAsync();
    }
    // public async Task<bool> CreateItemAsync(Item item)
    // {
    //     const string sql = "INSERT INTO \"Items\" (\"Name\", \"Description\") VALUES (@Name, @Description)";
    //     await _context.Items.FromSqlRaw()
    //     var result = await connection.ExecuteAsync(sql, item);
    //     return result > 0;
    // }

    // public async Task<bool> UpdateItemAsync(Item item)
    // {
    //     const string sql = "UPDATE \"Items\" SET \"Name\" = @Name, \"Description\" = @Description WHERE Id = @Id";
    //     using var connection = await _dapperContext.CreateConnectionAsync();
    //     var result = await connection.ExecuteAsync(sql, item);
    //     return result > 0;
    // }

    // public async Task<bool> DeleteItemAsync(int id)
    // {
    //     const string sql = "DELETE FROM \"Items\" WHERE \"Id\" = @Id";
    //     using var connection = await _dapperContext.CreateConnectionAsync();
    //     var result = await connection.ExecuteAsync(sql, new { Id = id });
    //     return result > 0;
    // }
}
