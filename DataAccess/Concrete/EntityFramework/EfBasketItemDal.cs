using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
	public class EfBasketItemDal : EfEntityRepositoryBase<BasketItem, MumYolContext>, IBasketItemDal
	{
		public List<BasketDetailsDto> GetBasketDetails(int userId)
		{
			using (var context = new MumYolContext())
			{
				var result = from b in context.BasketItems
							 join p in context.Products
							 on b.ProductId equals p.Id
							 where b.UserId == userId
							 select new BasketDetailsDto
							 {
								Brand = p.Brand,
								CategoryId = p.CategoryId,
								Description = p.Description,
								IsActive = p.IsActive,
								MinQuantityForWholesale = p.MinQuantityForWholesale,
								Name = p.Name,
								Quantity = b.Quantity,
								RetailPrice = p.RetailPrice,
								Unit = p.Unit,
								WholesalePrice = p.WholesalePrice,
							 };
				return result.ToList();
			}
		}
	}
}
