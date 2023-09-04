namespace top_online_store_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private EntityGateway _db = new();

        /// <summary>
        /// Return info about existing providers
        /// </summary>
        [HttpGet]
        public IActionResult GetAllProviders() => Ok(new
        {
            status = "ok",
            provider = _db.GetTable<Provider>().Select(x => new
            {
                x.Id,
                x.Name
            })
        });

        /// <summary>
        /// Get full info about provider
        /// </summary>
        [HttpGet]
        [Route("{provider_id:guid}")]
        [Route("api/products/{product_id:guid}/providers")]
        public IActionResult GetProviderById(Guid provider_id, Guid? product_id = null)
        {
            try
            {
                var potentialProduct = _db.GetTable<Product>().FirstOrDefault(x => x.Id == product_id);
                if (potentialProduct is null)
                    throw new KeyNotFoundException($"Product with id {product_id} is not found");

                var potentialProvider = _db.GetTable<Provider>().FirstOrDefault(x => x.Id == provider_id);
                if (potentialProvider is null)
                    return NotFound(new
                    {
                        status = "fail",
                        message = $"Provider with id {provider_id} is not found!"
                    });
                else
                    return Ok(new
                    {
                        status = "ok",
                        provider_info = potentialProvider,
                        products = potentialProvider.Products.Select(x => x.Name)
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
        /// Save new provider (id should be absent) or update an existing one
        /// </summary>
        [HttpPost]
        public IActionResult PostProvider([FromBody] JObject providerJson)
        {
            try
            {
                var provider = providerJson.ToObject<Provider>() ?? throw new Exception("Could not deserialize your object!");
                _db.AddOrUpgrade(provider);
                return Ok(new
                {
                    status = "Ok",
                    id = provider.Id
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
        /// Delete provider
        /// </summary>
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteProviderById(Guid id)
        {
            var potentialProvider = _db.GetTable<Provider>().FirstOrDefault(x => x.Id == id);
            if (potentialProvider is null)
                return NotFound(new
                {
                    status = "fail",
                    message = $"Provider with id {id} is not found!"
                });
            else
            {
                _db.Delete(potentialProvider);
                return Ok(new
                {
                    status = "ok",
                });
            }
        }
    }
}
