using Bussiness;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

		[HttpGet]
        [Authorize(Roles = "Admin")]
		public List<Order> GetAll()
		{
			return _orderService.ReadAll().ToList();
		}

		[HttpGet("{orderId}")]
        public List<OrderDetail> GetByOrderId([FromRoute] int orderId)
        {
            //return _orderService.ReadByIdAndCustomerId(orderId, customerId);
            return _orderService.GetOrderDetail(orderId);
        }

        [HttpGet("customers/{customerId}")]
        public List<Order> GetByCustomerId([FromRoute] Guid customerId)
        {
            return _orderService.GetByCustomerId(customerId);
        }

        [HttpPost]
        public void Create([FromBody] Order order)
        {
            _orderService.Create(order);
        }

        [HttpPut("{orderId}")]
        public ActionResult Update([FromRoute] int orderId, [FromBody] Order order)
        {
            _orderService.Update(orderId, order);
            return Ok();
        }

        //[HttpDelete("{orderId}")]
        //public ActionResult Delete([FromRoute] int orderId)
        //{
        //    _orderService.Delete(orderId);
        //    return Ok();
        //}

        [HttpGet("/customers/{customerId}/flower/{flowerId}")]
        public void AddFlower([FromRoute] Guid customerId, [FromRoute] int flowerId)
        {
            _orderService.AddFlower(customerId, flowerId);
		}

        [HttpPut("make-done/{orderId}")]
        public ActionResult MakeDone([FromRoute] int orderId)
        {
            _orderService.MakeDone(orderId);
            return Ok();
        }

		[HttpDelete("{orderId}")]
		public void Delete([FromRoute] int orderId)
		{
			_orderService.Delete(orderId);
		}
	}
}
