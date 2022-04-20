using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Integrations;
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
        IBalanceHistoryService _balanceHistoryService;
        public OrderManager(IOrderDal orderDal, IUserService userService, IOrderItemDal orderItemDal, IBasketService basketService, IProductService productService, IBalanceHistoryService balanceHistoryService)
        {
            _orderDal = orderDal;
            _userService = userService;
            _orderItemDal = orderItemDal;
            _basketService = basketService;
            _productService = productService;
            _balanceHistoryService = balanceHistoryService;
        }

        [SecuredOperation("personnel,admin")]
        public IDataResult<List<InsufficientStock>> GetAllInsufficientStocks()
        {
            var orders = _orderDal.GetAll(o => o.OrderStatus.Equals((int)OrderStatusEnum.Waiting) || o.OrderStatus.Equals((int)OrderStatusEnum.Approved));
            var orderItems = new List<OrderItem>();
            foreach (var order in orders)
            {
                orderItems.AddRange(_orderItemDal.GetAll(oi => oi.OrderId.Equals(order.Id)));
            }
            orderItems = orderItems.GroupBy(p => p.ProductId).Select(oi => new OrderItem
            {
                ProductId = oi.First().ProductId,
                Quantity = oi.Sum(o => o.Quantity),
                
            }).ToList();
            var insufficientStocks = new List<InsufficientStock>();
            foreach (var orderItem in orderItems)
            {
                var product = _productService.GetById(orderItem.ProductId);
                if (!product.Success) return new ErrorDataResult<List<InsufficientStock>>(product.Message);
                var insufficientStock = new InsufficientStock();
                insufficientStock.ProductId = orderItem.ProductId;
                insufficientStock.ProductName = product.Data.Name;
                insufficientStock.RequiredStock = orderItem.Quantity;
                insufficientStock.StockAmount = product.Data.StockAmount;
                insufficientStocks.Add(insufficientStock);
            }
            insufficientStocks = insufficientStocks.Where(i => i.RequiredStock > i.StockAmount).ToList();
            return new SuccessDataResult<List<InsufficientStock>>(insufficientStocks);
        }

        [SecuredOperation("personnel,admin")]
        public IDataResult<List<OrderDetailsDto>> GetAllOrdersByStatusId(int statusId)
        {
            var orderDetailsDtos = new List<OrderDetailsDto>();
            var orderDetailsDto = new OrderDetailsDto();

            var orders = _orderDal.GetAll(o => statusId == 0 ? o.OrderStatus.Equals(statusId) : true);

            foreach (var order in orders)
            {
                orderDetailsDto.OrderId = order.Id;
                orderDetailsDto.CreatedDate = order.CreatedDate;
                orderDetailsDto.UserId = order.UserId;
                orderDetailsDto.OrderStatus = order.OrderStatus;
                orderDetailsDto.PayBack = order.PayBack;
                orderDetailsDto.OrderItems = _orderItemDal.GetAllOrderItemDetails(order.Id);
                orderDetailsDtos.Add(orderDetailsDto);
            }
            return new SuccessDataResult<List<OrderDetailsDto>>(orderDetailsDtos);
        }

        [SecuredOperation("user,personnel,admin")]
        public IDataResult<List<OrderDetailsDto>> GetAllOrderDetailsByUser()
        {
            var orderDetailsDtos = new List<OrderDetailsDto>();
            var user = _userService.GetUser();
            if(user == null) return new ErrorDataResult<List<OrderDetailsDto>>("Kullanici bulunamadi!");
            var orders = _orderDal.GetAll(o => o.UserId.Equals(user.Id));

            foreach (var order in orders)
            {
                var orderDetailsDto = new OrderDetailsDto();
                orderDetailsDto.OrderId = order.Id;
                orderDetailsDto.CreatedDate = order.CreatedDate;
                orderDetailsDto.UserId = order.UserId;
                orderDetailsDto.OrderStatus = order.OrderStatus;
                orderDetailsDto.PayBack = order.PayBack;
                orderDetailsDto.OrderItems = _orderItemDal.GetAllOrderItemDetails(order.Id);
                orderDetailsDtos.Add(orderDetailsDto);
            }
            return new SuccessDataResult<List<OrderDetailsDto>>(orderDetailsDtos);
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
            orderDetailsDto.PayBack = order.PayBack;
            orderDetailsDto.OrderItems = _orderItemDal.GetAllOrderItemDetails(orderId);

            return new SuccessDataResult<OrderDetailsDto>(orderDetailsDto);
        }

        [SecuredOperation("user,personnel,admin")]
        public IDataResult<List<Order>> GetOrdersByUser()
        {
            return new SuccessDataResult<List<Order>>(_orderDal.GetAll(o => o.UserId.Equals(_userService.GetUser().Id)));
        }

        [SecuredOperation("user,personnel,admin")]
        public IDataResult<int> OrderBasket()
        {
            Decimal totalPrice = 0;
            var basketItems = _basketService.GetAll();
            if (basketItems.Data.Count == 0)
                return new ErrorDataResult<int>("Sepet Bos!");

            var orderItems = new List<OrderItem>();
            foreach (var item in basketItems.Data)
            {
                var product = _productService.GetById(item.ProductId);
                if (!product.Data.IsActive)
                {
                    return new ErrorDataResult<int>("Sepetinizde satista olmayan urun var!");
                }
                var orderItem = new OrderItem()
                {
                    OrderId = 0,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Quantity > product.Data.MinQuantityForWholesale ? product.Data.WholesalePrice : product.Data.RetailPrice,
                    PayBackRate = product.Data.PayBackRate,
                    PurchasePrice = product.Data.PurchasePrice,
                };
                totalPrice += orderItem.Quantity * orderItem.Price;
                orderItems.Add(orderItem);
            }

            var user =_userService.GetUser();
            var order = new Order();
            order.UserId = user.Id;
            order.CreatedDate = DateTime.Now;
            order.OrderStatus = (int)OrderStatusEnum.Waiting;
            order.PayBack = 0;
            _orderDal.Add(order);

            foreach (var item in orderItems)
            {
                item.OrderId = order.Id;
                _orderItemDal.Add(item);
            }
            var admin = _userService.GetAdmin();
            var msg = string.Format(Messages.NewOrderMessage, admin.FirstName + " " + admin.LastName, DateTime.Now.ToString("dd/MM/yyyy HH:mm"), user.FirstName + " " + user.LastName);
            SmsIntegration.SendSms(admin.PhoneNumber, msg);

            _basketService.DeleteAll(); // Clear basket

            return new SuccessDataResult<int>(order.Id);
        }

        [SecuredOperation("personnel,admin")]
        public IResult UpdateOrderStatus(UpdateOrderStatusDto updateOrderStatusDto)
        {
            var order = _orderDal.Get(o => o.Id.Equals(updateOrderStatusDto.OrderId));
            if(order == null) return new ErrorResult("Siparis bulunamadi!");

            if (!Enum.GetValues(typeof(OrderStatusEnum))
                            .Cast<int>()
                            .ToList().Contains(updateOrderStatusDto.StatusId))
                return new ErrorResult("Hatalı durum seçildi!");

            if (order.OrderStatus == (int)OrderStatusEnum.Completed)
                return new ErrorResult("Siparis zaten tamamlanmis!");

            if (order.OrderStatus == (int)OrderStatusEnum.Approved && updateOrderStatusDto.StatusId == (int)OrderStatusEnum.Waiting)
                return new ErrorResult("Siparis zaten onaylanmis! Bu islem yapilamaz");

            if (updateOrderStatusDto.StatusId == (int)OrderStatusEnum.Completed)
            {
                var user = _userService.GetUserById(order.UserId);
                if (user == null)
                    return new ErrorResult("Kullanici bulunamadi!");
                decimal payBack = 0;
                var orderItems = _orderItemDal.GetAll(oi => oi.OrderId == order.Id);
                foreach (var item in orderItems)
                {
                    payBack += (item.Price - item.PurchasePrice) * (item.PayBackRate / 100);
                }
                var balanceHistory = new BalanceHistory
                {
                    Money = payBack,
                    BalanceAfter = user.Balance + payBack,
                    Date = DateTime.Now,
                    UserId = order.UserId,
                };
                order.PayBack = payBack;
                _balanceHistoryService.Add(balanceHistory);
                user.Balance += payBack;
                _userService.Update(user);
            }

            order.OrderStatus = updateOrderStatusDto.StatusId;
            _orderDal.Update(order);
            return new SuccessResult("Durum guncellendi!");
        }
    }
}
