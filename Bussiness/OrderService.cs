using AutoMapper;
using Common.ExceptionHandler.Exceptions;
using DataAccess;
using DataAccess.Models;

namespace Bussiness
{
	public class OrderService
	{
		private readonly IGenericRep<Order> _OrderRep;
		private readonly IGenericRep<FlowerBouquet> _FlowerBouquet;
		private readonly IGenericRep<OrderDetail> _OrderDetail;
		private readonly IMapper _mapper;
		public OrderService(IGenericRep<Order> OrderRep, IGenericRep<FlowerBouquet> FlowerBouquet, IGenericRep<OrderDetail> OrderDetail, IMapper mapper)
		{
			this._OrderRep = OrderRep;
			this._OrderDetail = OrderDetail;
			this._FlowerBouquet = FlowerBouquet;
			this._mapper = mapper;
		}

		public Order Read(int id)
		{
			var Orders = _OrderRep.All;
			if (Orders == null)
			{
				throw new BadRequestException("Order Not Found!");
			}
			var response = Orders.FirstOrDefault(it => it.OrderId == id);
			if (response == null)
			{
				throw new BadRequestException("Order Not Found!");
			}
			return response;
		}

		public void Delete(int id)
		{
			var Orders = _OrderRep.All;
			if (Orders == null)
			{
				return;
			}
			var response = Orders.FirstOrDefault(it => it.OrderId == id);
			if (response == null)
			{
				return;
			}
			response.OrderStatus = "Delete";
			_OrderRep.Update(response);
		}

		public List<Order> GetByCustomerId(Guid customerId)
		{
			var Orders = _OrderRep.All;
			if (Orders == null)
			{
				return new List<Order>();
			}
			var response = Orders.Where(it => it.CustomerId == customerId);
			if (response == null)
			{
				return new List<Order>();
			}
			return response.ToList();
		}

		public List<Order> ReadAll()
		{
			var Orders = _OrderRep.All;
			if (Orders == null)
			{
				return new List<Order>();
			}
			return Orders.ToList();
		}

		public void MakeDone(int orderId)
		{
			var Orders = _OrderRep.All;
			if (Orders == null)
			{
				throw new BadRequestException("Order Not Found!");
			}
			var response = Orders.FirstOrDefault(it => it.OrderId == orderId);
			if (response == null)
			{
				throw new BadRequestException("Order Not Found!");
			}
			response.OrderStatus = "Done";
			_OrderRep.Update(response);
		}

		public void Create(Order Order)
		{
			_OrderRep.Create(Order);
		}

		public void Update(int id, Order Order)
		{
			var Orders = _OrderRep.All;
			if (Orders == null)
			{
				throw new BadRequestException("Order Not Found!");
			}
			var response = Orders.FirstOrDefault(it => it.OrderId == id);
			if (response == null)
			{
				throw new BadRequestException("Order Not Found!");
			}
			_mapper.Map(Order, response);
			_OrderRep.Update(response);
		}

		//public void Delete(int id)
		//{
		//	var Orders = _OrderRep.All;
		//	if (Orders == null)
		//	{
		//		throw new BadRequestException("Order Not Found!");
		//	}
		//	var response = Orders.FirstOrDefault(it => it.OrderId == id);
		//	if (response == null)
		//	{
		//		throw new BadRequestException("Order Not Found!");
		//	}
		//	_OrderRep.Delete(response);
		//}

		public Order ReadByIdAndCustomerId(int orderId, Guid customerId)
		{
			var Orders = _OrderRep.All;
			if (Orders == null)
			{
				return null;
			}
			var response = Orders.FirstOrDefault(it => it.OrderId == orderId && it.CustomerId == customerId);
			if (response == null)
			{
				return null;
			}
			return response;
		}

		public void AddFlower(Guid customerId, int flowerId)
		{
			using (var context = new FUFlowerBouquetManagementContext())
			{
				var order = context.Orders.FirstOrDefault(o => o.CustomerId == customerId && o.OrderStatus.Trim().Equals("Waiting"));

				if (order == null)
				{
					order = new Order
					{
						OrderStatus = "Waiting",
						OrderDate = DateTime.Now,
						CustomerId = customerId,
						ShippedDate = DateTime.Now
					};

					context.Orders.Add(order);
					context.SaveChanges();
				}

				var flower = context.FlowerBouquets.FirstOrDefault(f => f.FlowerBouquetId == flowerId);

				if (flower != null)
				{
					var orderDetail = context.OrderDetails.FirstOrDefault(od => od.FlowerBouquetId == flowerId && od.OrderId == order.OrderId);

					if (orderDetail == null)
					{
						context.OrderDetails.Add(new OrderDetail
						{
							UnitPrice = flower.UnitPrice,
							Quantity = 1,
							FlowerBouquetId = flowerId,
							OrderId = order.OrderId
						});
					}
					else
					{
						orderDetail.Quantity += 1;
						context.OrderDetails.Update(orderDetail);
					}

					order.Total += flower.UnitPrice;
					context.SaveChanges();
				}
			}
		}

		public List<OrderDetail> GetOrderDetail(int orderId)
		{
			var orderDetails = _OrderDetail.All;
			if (orderDetails == null)
			{
				return new List<OrderDetail>();
			}

			var response = orderDetails.Where(it => it.OrderId == orderId);
			if(response == null || response.Count() == 0)
			{
				return new List<OrderDetail>();
			}
			return response.ToList();
		}
	}
}
