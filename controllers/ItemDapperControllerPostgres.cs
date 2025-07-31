using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace product_api_dotnet8.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemDapperControllerPostgres : ControllerBase
    {
        private readonly DataContextPostgres _dataContext;

        public ItemDapperControllerPostgres(DataContextPostgres dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            try
            {
                var items = await _dataContext.GetAllItemsAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching items", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            try
            {
                var item = await _dataContext.GetItemByIdAsync(id);
                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching item", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateItem(Item item)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _dataContext.CreateItemAsync(item);
                if (!success) return StatusCode(500, "Failed to create item");
                return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating item", error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateItem(Item item)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _dataContext.UpdateItemAsync(item);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating item", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                var success = await _dataContext.DeleteItemAsync(id);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting item", error = ex.Message });
            }
        }
    }
}
