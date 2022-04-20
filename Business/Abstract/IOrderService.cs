using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IOrderService
    {
        IDataResult<int> OrderBasket();
        IDataResult<OrderDetailsDto> GetOrderDetailsById(int orderId);
        IDataResult<List<OrderDetailsDto>> GetAllOrdersByStatusId(int statusId);
        IDataResult<List<OrderDetailsDto>> GetAllOrderDetailsByUser();
        IDataResult<List<Order>> GetOrdersByUser();
        IDataResult<List<InsufficientStock>> GetAllInsufficientStocks();
        IResult UpdateOrderStatus(UpdateOrderStatusDto updateOrderStatusDto);
    }
}
