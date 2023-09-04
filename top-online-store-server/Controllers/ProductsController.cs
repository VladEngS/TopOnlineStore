namespace top_online_store_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private EntityGateway _db = new();

        /// <summary>
        /// Return info about existing products
        /// </summary>
        [HttpGet]
        public IActionResult GetAllProducts() => Ok(new
        {
            status = "ok",
            products = _db.GetTable<Product>().Select(x => new
            {
                x.Id,
                x.Name,
                x.Category
            })
        });

        /// <summary>
        /// Get full info about product
        /// </summary>
        [HttpGet]
        [Route("{product_id:guid}")]
        [Route("api/providers/{provider_id:guid}/products")]
        [Route("api/storages/{storage_id:guid}/products")]
        [Route("api/orders/{order_id:guid}/products")]
        public IActionResult GetProductById(Guid product_id, Guid? provider_id = null, Guid? storage_id = null, Guid? order_id = null)
        {
            try
            {
                var potentialProvider = _db.GetTable<Provider>().FirstOrDefault(x => x.Id == provider_id);
                if (potentialProvider is null)
                    throw new KeyNotFoundException($"Provider with id {provider_id} is not found");

                var potentialStorage = _db.GetTable<Storage>().FirstOrDefault(x => x.Id == storage_id);
                if (potentialStorage is null)
                    throw new KeyNotFoundException($"Storage with id {storage_id} is not found");

                var potentialOrder = _db.GetTable<Order>().FirstOrDefault(x => x.Id == order_id);
                if (potentialOrder is null)
                    throw new KeyNotFoundException($"Order with id {order_id} is not found");

                var potentialProduct = _db.GetTable<Product>().FirstOrDefault(x => x.Id == product_id);
                if (potentialProduct is null)
                    return NotFound(new
                    {
                        status = "fail",
                        message = $"Product with id {product_id} is not found!"
                    });
                else
                    return Ok(new
                    {
                        status = "ok",
                        product_info = potentialProduct,
                        provider = potentialProduct.Provider.Name,

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
        /// Save new product (id should be absent) or update an existing one
        /// </summary>
        [HttpPost]
        public IActionResult PostProduct([FromBody] JObject productJson)
        {
            try
            {
                var product = productJson.ToObject<Product>() ?? throw new Exception("Could not deserialize your object!");
                _db.AddOrUpgrade(product);
                return Ok(new
                {
                    status = "Ok",
                    id = product.Id
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
        /// Delete product
        /// </summary>
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteProductById(Guid id)
        {
            var potentialProduct = _db.GetTable<Product>().FirstOrDefault(x => x.Id == id);
            if (potentialProduct is null)
                return NotFound(new
                {
                    status = "fail",
                    message = $"Product with id {id} is not found!"
                });
            else
            {
                _db.Delete(potentialProduct);
                return Ok(new
                {
                    status = "ok",
                });
            }
        }

    }
}
