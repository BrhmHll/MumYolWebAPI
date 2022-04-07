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
        IDataResult<OrderDetailsDto> OrderBasket();
        IDataResult<OrderDetailsDto> GetOrderDetailsById(int orderId);
        IDataResult<List<Order>> GetOrdersByUser();
        IResult UpdateOrderStatus(UpdateOrderStatusDto updateOrderStatusDto);
    }
}
