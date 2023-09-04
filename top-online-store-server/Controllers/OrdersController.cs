namespace top_online_store_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private EntityGateway _db = new();

        /// <summary>
        /// Get full info about order
        /// </summary>
        [HttpGet]
        [Route("{order_id:guid}")]
        [Route("api/clients/{client_id:guid}/orders")]
        public IActionResult GetOrderById(Guid order_id, Guid? client_id = null)
        {
            try
            {
                var potentialClient = _db.GetTable<Client>().FirstOrDefault(x => x.Id == client_id);
                if (potentialClient is null)
                    throw new KeyNotFoundException($"Client with id {client_id} is not found");

                var potentialOrder = _db.GetTable<Order>().FirstOrDefault(x => x.Id == order_id);
                if (potentialOrder is null)
                    return NotFound(new
                    {
                        status = "fail",
                        message = $"Order with id {order_id} is not found!"
                    });
                else
                    return Ok(new
                    {
                        status = "ok",
                        order_info = potentialOrder,
                        client_info = potentialClient.Surname
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
        /// Save new order (id should be absent) or update an existing one
        /// </summary>
        [HttpPost]
        public IActionResult PostOrder([FromBody] JObject orderJson)
        {
            try
            {
                var order = orderJson.ToObject<Order>() ?? throw new Exception("Could not deserialize your object!");
                _db.AddOrUpgrade(order);
                return Ok(new
                {
                    status = "Ok",
                    id = order.Id
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
        /// Delete order
        /// </summary>
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteOrderById(Guid id)
        {
            var potentialOrder = _db.GetTable<Order>().FirstOrDefault(x => x.Id == id);
            if (potentialOrder is null)
                return NotFound(new
                {
                    status = "fail",
                    message = $"Order with id {id} is not found!"
                });
            else
            {
                _db.Delete(potentialOrder);
                return Ok(new
                {
                    status = "ok",
                });
            }
        }
    }
}
