using AutoMapper;
using Common.ExceptionHandler.Exceptions;
using DataAccess;
using DataAccess.Models;

namespace Bussiness
{
    public class OrderDetailService
    {
        private readonly IGenericRep<OrderDetail> _OrderDetailRep;
        private readonly IGenericRep<Order> _OrderRep;
        private readonly IMapper _mapper;
        public OrderDetailService(IGenericRep<OrderDetail> OrderDetailRep, IGenericRep<Order> OrderRep, IMapper mapper)
        {
            this._OrderDetailRep = OrderDetailRep;
            this._OrderRep = OrderRep;
            this._mapper = mapper;
        }

        public List<OrderDetail> ReadByOrderId(int orderId)
        {
            var orderDetails = _OrderDetailRep.All;
            if (orderDetails == null)
            {
                return new List<OrderDetail>();
            }
            else
            {
                return orderDetails.Where(it => it.OrderId == orderId).ToList();
            }
        }

        public void Create(OrderDetail OrderDetail)
        {
            var Orders = _OrderRep.All;
            if(Orders == null) {
                throw new BadRequestException("Order Not Found");
            }
            var Order = Orders.FirstOrDefault(it => it.OrderId == OrderDetail.OrderId);
            if (Order == null)
            {
                throw new BadRequestException("Order Not Found");
            }

            Order.Total += OrderDetail.UnitPrice;
            _OrderDetailRep.Create(OrderDetail);
            _OrderRep.Update(Order);
            
        }

        public void Delete(int orderId, int flowerId)
        {
            var orderDetails = _OrderDetailRep.All;
            if (orderDetails == null)
            {
                throw new BadRequestException("OrderDetail Not Found!");
            }
            var response = orderDetails.FirstOrDefault(it => it.OrderId == orderId && it.FlowerBouquetId == flowerId);
            if (response == null)
            {
                throw new BadRequestException("OrderDetail Not Found!");
            }
            _OrderDetailRep.Delete(response);
        }

        public void DeleteByOrderId(int orderId)
        {
            var orderDetails = _OrderDetailRep.All;
            if (orderDetails == null)
            {
                return;
            }
            var responses = orderDetails.Where(it => it.OrderId == orderId).ToList();
            if (responses == null)
            {
                return;
            }
            responses.ForEach(it =>
            {
                _OrderDetailRep.Delete(it);
            });
        }
    }
}
