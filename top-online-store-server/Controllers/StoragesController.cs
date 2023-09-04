namespace top_online_store_server.Controllers
{
    /// <summary>
    /// Set of methods to manipulate storages
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StoragesController : ControllerBase
    {
        private EntityGateway _db = new();

        /// <summary>
        /// Return info about existing storages
        /// </summary>
        [HttpGet]
        public IActionResult GetAllStorages() => Ok(new
        {
            status = "ok",
            storages = _db.GetTable<Storage>().Select(x => new
            {
                x.Id,
                x.Address,
            })
        });

        /// <summary>
        /// Get full info about storage
        /// </summary>
        [HttpGet]
        [Route("{storage_id:guid}")]
        [Route("api/products/{product_id:guid}/storages")]
        public IActionResult GetStorageById(Guid storage_id, Guid? product_id = null)
        {
            try
            {
                var potentialProduct = _db.GetTable<Product>().FirstOrDefault(x => x.Id == product_id);
                if (potentialProduct is null)
                    throw new KeyNotFoundException($"Product with id {product_id} is not found");

                var potentialStorage = _db.GetTable<Storage>().FirstOrDefault(x => x.Id == storage_id);
                if (potentialStorage is null)
                    return NotFound(new
                    {
                        status = "fail",
                        message = $"Storage with id {storage_id} is not found!"
                    });
                else
                    return Ok(new
                    {
                        status = "ok",
                        storage_info = potentialStorage,
                        products = potentialStorage.Products.Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Provider
                        })
                    });
            }
            catch (KeyNotFoundException E)
            {
                return NotFound(new
                {
                    status = "fail",
                    message = E.Message
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
        /// Save new storage (id should be absent) or update an existing one
        /// </summary>
        [HttpPost]
        public IActionResult PostStorage([FromBody] JObject storageJson)
        {
            try
            {
                var storage = storageJson.ToObject<Storage>() ?? throw new Exception("Could not deserialize your object!");
                _db.AddOrUpgrade(storage);
                return Ok(new
                {
                    status = "Ok",
                    id = storage.Id
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
        /// Delete storage
        /// </summary>
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteStorageById(Guid id)
        {
            var potentialStorage = _db.GetTable<Storage>().FirstOrDefault(x => x.Id == id);
            if (potentialStorage is null)
                return NotFound(new
                {
                    status = "fail",
                    message = $"Storage with id {id} is not found!"
                });
            else
            {
                _db.Delete(potentialStorage);
                return Ok(new
                {
                    status = "ok",
                });
            }
        }
    }
}
