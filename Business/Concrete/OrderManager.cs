using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class OrderManager : IOrderService
    {
        IOrderDal _orderDal;
        IOrderItemDal _orderItemDal;
        IUserService _userService;
        IBasketService _basketService;
        IProductService _productService;
        public OrderManager(IOrderDal orderDal, IUserService userService, IOrderItemDal orderItemDal, IBasketService basketService, IProductService productService)
        {
            _orderDal = orderDal;
            _userService = userService;
            _orderItemDal = orderItemDal;
            _basketService = basketService;
            _productService = productService;
        }

        [SecuredOperation("user,personnel,admin")]
        public IDataResult<OrderDetailsDto> GetOrderDetailsById(int orderId)
        {
            var order = _orderDal.Get(o => o.Id.Equals(orderId) && o.UserId.Equals(_userService.GetUser().Id));
            if (order == null)
                return new ErrorDataResult<OrderDetailsDto>("Siparis bulunamadi!");

            var orderDetailsDto = new OrderDetailsDto();

            orderDetailsDto.OrderId = orderId;
            orderDetailsDto.CreatedDate = order.CreatedDate;
            orderDetailsDto.UserId = order.UserId;
            orderDetailsDto.OrderStatus = order.OrderStatus;
            orderDetailsDto.OrderItems = _orderItemDal.GetAll(i => i.OrderId.Equals(orderId));

            return new SuccessDataResult<OrderDetailsDto>(orderDetailsDto);
        }

        [SecuredOperation("user,personnel,admin")]
        public IDataResult<List<Order>> GetOrdersByUser()
        {
            return new SuccessDataResult<List<Order>>(_orderDal.GetAll(o => o.UserId.Equals(_userService.GetUser().Id)));
        }

        [SecuredOperation("user,personnel,admin")]
        public IDataResult<OrderDetailsDto> OrderBasket()
        {
            var basketItems = _basketService.GetAll();
            if (basketItems.Data.Count == 0)
                return new ErrorDataResult<OrderDetailsDto>("Sepet Bos!");

            var orderItems = new List<OrderItem>();
            foreach (var item in basketItems.Data)
            {
                var product = _productService.GetById(item.ProductId);
                if (!product.Data.IsActive)
                {
                    return new ErrorDataResult<OrderDetailsDto>("Sepetinizde satista olmayan urun var!");
                }
                var orderItem = new OrderItem()
                {
                    OrderId = 1,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Quantity > product.Data.MinQuantityForWholesale ? product.Data.WholesalePrice : product.Data.RetailPrice
                };
                orderItems.Add(orderItem);
            }

            var order = new Order();
            order.UserId = _userService.GetUser().Id;
            order.CreatedDate = DateTime.Now;
            order.OrderStatus = (int)OrderStatusEnum.Waiting;
            _orderDal.Add(order);

            foreach (var item in orderItems)
            {
                item.OrderId = order.Id;
                _orderItemDal.Add(item);
            }

            _basketService.DeleteAll(); // Clear basket

            var orderDetailsDto = new OrderDetailsDto()
            {
                CreatedDate = order.CreatedDate,
                OrderId = order.Id,
                OrderItems = orderItems,
                OrderStatus = order.OrderStatus,
                UserId = order.UserId
            };
            return new SuccessDataResult<OrderDetailsDto>(orderDetailsDto);
        }

        [SecuredOperation("personnel,admin")]
        public IResult UpdateOrderStatus(UpdateOrderStatusDto updateOrderStatusDto)
        {
            var order = _orderDal.Get(o => o.Id.Equals(updateOrderStatusDto.OrderId) && o.UserId.Equals(_userService.GetUser().Id));
            if(order == null) return new ErrorResult("Siparis bulunamadi!");

            order.OrderStatus = updateOrderStatusDto.StatusId;
            _orderDal.Update(order);
            return new SuccessResult("Durum guncellendi!");
        }
    }
}
