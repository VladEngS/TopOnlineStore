namespace top_online_store_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private EntityGateway _db = new();

        /// <summary>
        /// Return info about existing categories
        /// </summary>
        [HttpGet]
        public IActionResult GetAllCategories() => Ok(new
        {
            status = "ok",
            categories = _db.GetTable<Category>().Select(x => new
            {
                x.Id,
                x.Name,
            })
        });

        /// <summary>
        /// Save new category (id should be absent) or update an existing one
        /// </summary>
        [HttpPost]
        public IActionResult PostCategory([FromBody] JObject categoryJson)
        {
            try
            {
                var category = categoryJson.ToObject<Category>() ?? throw new Exception("Could not deserialize your object!");
                _db.AddOrUpgrade(category);
                return Ok(new
                {
                    status = "Ok",
                    id = category.Id
                });
            }
            catch (Exception E)
            {
                return BadRequest(new
                {
                    status = "fail",
                    message = E.Message
                });
            }
        }

        /// <summary>
        /// Delete category
        /// </summary>
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteCategoryById(Guid id)
        {
            var potentialCategory = _db.GetTable<Category>().FirstOrDefault(x => x.Id == id);
            if (potentialCategory is null)
                return NotFound(new
                {
                    status = "fail",
                    message = $"Category with id {id} is not found!"
                });
            else
            {
                _db.Delete(potentialCategory);
                return Ok(new
                {
                    status = "ok",
                });
            }
        }
    }
}
