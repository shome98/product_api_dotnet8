using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace product_api_dotnet8.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        static private List<Item> itemList = new List<Item>
            {
                new Item{Id=1,Name="Item 1",Description="Item 1 Description"},
                new Item{Id=2,Name="Item 2",Description="Item 2 Description"},
                new Item{Id=3,Name="Item 3",Description="Item 3 Description"},
                new Item{Id=4,Name="Item 4",Description="Item 4 Description"},
                new Item{Id=5,Name="Item 5",Description="Item 5 Description"},
                new Item{Id=6,Name="Item 6",Description="Item 6 Description"},
            };

        [HttpGet("getdata")]
        public string GetApidata()
        {
            return "you have visited the api😊 ";
        }

        [HttpGet("getitems")]
        public ActionResult<List<Item>> GetApiitems()
        {

            return Ok(itemList);
        }
        [HttpGet("getitemById/{id}")]
        public ActionResult<Item> GetItemById(int id)
        {
            var item = itemList.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                return Ok(item);

            }
            return NotFound("Item not found");
        }

    }
}
