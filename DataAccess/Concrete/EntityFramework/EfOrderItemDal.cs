using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfOrderItemDal : EfEntityRepositoryBase<OrderItem, MumYolContext>, IOrderItemDal
    {
        public List<OrderItemDetail> GetAllOrderItemDetails(int orderId)
        {
			using (var context = new MumYolContext())
			{
				var result = from oi in context.OrderItems
							 where oi.OrderId == orderId
							 select new OrderItemDetail
							 {
								 Id = oi.Id,
								 OrderId = oi.OrderId,
								 Price = oi.Price,
								 ProductId = oi.ProductId,
								 Quantity = oi.Quantity,
								 ImagePath = (from i in context.ProductImages where oi.ProductId == i.ProductId select i.ImagePath).FirstOrDefault(),
								 PayBackRate = oi.PayBackRate,
								 PurchasePrice = oi.PurchasePrice,
							 };
				return result.ToList();

			}
		}
    }
}
