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
}