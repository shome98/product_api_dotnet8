using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using product_api_dotnet8.Db;

namespace product_api_dotnet8.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemDapperController : ControllerBase
    {
        // DataContextDapper _dapper;
        // public ItemDapperController(IConfiguration config)
        // {
        //     _dapper = new DataContextDapper(config);
        // }


        // private readonly ILogger<DataContextDapper> _logger;

        // public DataContextDapper(DapperContext dapperContext, ILogger<DataContextDapper> logger)
        // {
        //     _dapperContext = dapperContext;
        //     _logger = logger;
        // }

        // Then in catch block:
        //_logger.LogError(ex, "Error executing query: {Message}", ex.Message);
        private readonly DataContextDapper _dataContext;
        public ItemDapperController(DataContextDapper dataContext)
        {
            _dataContext = dataContext;
        }

        // [HttpGet("items")]
        // public List<Item> GetItems()
        // {
        //     return _dapper.LoadData<Item>("SELECT * FROM items").ToList();
        // }

        // [HttpPost("items")]
        // public Task<Item> CreateItem(Item newItem)
        // {
        //     Item item = new Item{Id=newItem.Id,Name=newItem.Name,Description=newItem.Description};
        //     ;
        // }

        [HttpGet("items")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var items = await _dataContext.GetAllItemsAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching items", error = ex.Message });
                throw;
            }
        }

        [HttpGet("items/{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            try
            {
                var item = await _dataContext.GetItemByIdAsync(id);
                if (item == null) return NotFound(new { message = "Item not found" });
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching item", error = ex.Message });
            }
        }

        [HttpPost("items")]
        public async Task<IActionResult> CreateItem([FromBody] Item item)
        {
            if (item == null) return BadRequest(new { message = "Invalid Request, empty object was sent." });
            try
            {
                var success = await _dataContext.CreateItemAsync(item);
                if (!success) return StatusCode(500, new { message = "Failed to create item" });
                return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating item", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, [FromBody] Item item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest(new
                {
                    message = "Item data is invalid or ID mismatch",
                    details = new
                    {
                        urlId = id,
                        bodyId = item?.Id
                    }
                });
            }

            try
            {
                var success = await _dataContext.UpdateItemAsync(item);
                if (!success)
                {
                    return NotFound(new { message = "Item not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error updating item",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("itmes/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                var success = await _dataContext.DeleteItemAsync(id);
                if (!success) return NotFound(new { message = "Item not found" });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting item", error = ex.Message });
            }
        }
    }
}
