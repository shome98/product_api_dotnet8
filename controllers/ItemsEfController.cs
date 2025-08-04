using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using product_api_dotnet8.Db;

namespace product_api_dotnet8.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsEfController : ControllerBase
{
    private readonly DataContextEFPostgres _context;

    public ItemsEfController(DataContextEFPostgres context)
    {
        _context = context;
    }

    [HttpPost("createtable")]
    public async Task<IActionResult> CreateTable()
    {
        try
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS ""Items"" (
                    ""Id"" SERIAL PRIMARY KEY,
                    ""Name"" VARCHAR(255) NOT NULL,
                    ""Description"" TEXT
                );";

            await _context.Database.ExecuteSqlRawAsync(sql);

            return Ok(new
            {
                message = "Table 'Items' created successfully (or already exists)."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Failed to create table.",
                error = ex.Message
            });
        }
    }

    // GET: api/items
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item>>> GetItems()
    {
        try
        {
            var items = await _context.Items
                .FromSqlRaw("SELECT * FROM \"Items\"")
                .ToListAsync();

            return Ok(items);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error fetching items", error = ex.Message });
        }
    }

    // GET: api/items/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Item>> GetItem(int id)
    {
        try
        {
            var item = await _context.Items
                .FromSqlRaw("SELECT * FROM \"Items\" WHERE \"Id\" = {0}", id)
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(new { message = "Item not found" });

            return Ok(item);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error fetching item", error = ex.Message });
        }
    }

    // POST: api/items
    [HttpPost]
    public async Task<ActionResult> CreateItem(Item item)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var sql = "INSERT INTO \"Items\" (\"Name\", \"Description\") VALUES ({0}, {1})";
            var result = await _context.Database.ExecuteSqlRawAsync(sql, item.Name, item.Description);

            if (result > 0)
            {
                return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
            }

            return StatusCode(500, new { message = "Failed to create item" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating item", error = ex.Message });
        }
    }

    // PUT: api/items
    [HttpPut]
    public async Task<IActionResult> UpdateItem(Item item)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var sql = "UPDATE \"Items\" SET \"Name\" = {0}, \"Description\" = {1} WHERE \"Id\" = {2}";
            var result = await _context.Database.ExecuteSqlRawAsync(sql, item.Name, item.Description, item.Id);

            if (result == 0)
                return NotFound(new { message = "Item not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating item", error = ex.Message });
        }
    }

    // DELETE: api/items/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        try
        {
            var sql = "DELETE FROM \"Items\" WHERE \"Id\" = {0}";
            var result = await _context.Database.ExecuteSqlRawAsync(sql, id);

            if (result == 0)
                return NotFound(new { message = "Item not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error deleting item", error = ex.Message });
        }
    }

    
    [HttpPost("bulk-insert")]
    public async Task<IActionResult> CreateItemsBulk([FromBody] List<Item> items)
    {
        if (items == null || !items.Any())
            return BadRequest(new { message = "No items provided." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // Build a parameterized raw SQL query using UNION ALL
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO \"Items\" (\"Name\", \"Description\") VALUES ");

            var parameters = new List<object>();
            for (int i = 0; i < items.Count; i++)
            {
                sqlBuilder.Append($"({{0}}, {{1}})");
                parameters.Add(items[i].Name);
                parameters.Add(items[i].Description);

                if (i < items.Count - 1)
                    sqlBuilder.Append(", ");
            }

            var sql = sqlBuilder.ToString();
            await _context.Database.ExecuteSqlRawAsync(sql, parameters.ToArray());

            return StatusCode(201, new { message = $"{items.Count} items inserted successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Error inserting items",
                error = ex.Message
            });
        }
    }
}