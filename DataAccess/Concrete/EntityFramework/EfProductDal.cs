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
    public class EfProductDal : EfEntityRepositoryBase<Product, MumYolContext>, IProductDal
    {
        public List<ProductDetailDto> GetAllProductDetails(int categoryId)
        {
			using (var context = new MumYolContext())
			{
				var result = from p in context.Products
							 join c in context.Categories
							 on p.CategoryId equals c.Id
							 where categoryId != 0 ? p.CategoryId == categoryId : true
							 select new ProductDetailDto {
								 Id = p.Id,
								 Name = p.Name,
								 Brand = p.Brand,
								 CategoryId = p.CategoryId,
								 IsActive = p.IsActive,
								 Description = p.Description,
								 MinQuantityForWholesale = p.MinQuantityForWholesale,
								 RetailPrice = p.RetailPrice,
								 StockAmount = p.StockAmount,
								 Unit = p.Unit,
								 PayBackRate = p.PayBackRate,
								 PayBackRateWholesale = p.PayBackRateWholesale,
								 PurchasePrice = p.PurchasePrice,
								 WholesalePrice = p.WholesalePrice,
								 CategoryName = c.Name,
								 ImagePaths = (from pi in context.ProductImages
											  where pi.ProductId == p.Id
											  select pi.ImagePath).ToList()
							 };
				return result.ToList();

			}
		}

        public ProductDetailDto GetProductDetails(int productId)
        {
			using (var context = new MumYolContext())
			{
				var result = from p in context.Products
							 where p.Id == productId
							 select new ProductDetailDto
							 {
								 Id = p.Id,
								 Name = p.Name,
								 Brand = p.Brand,
								 CategoryId = p.CategoryId,
								 IsActive = p.IsActive,
								 Description = p.Description,
								 MinQuantityForWholesale = p.MinQuantityForWholesale,
								 RetailPrice = p.RetailPrice,
								 StockAmount = p.StockAmount,
								 Unit = p.Unit,
								 PayBackRate = p.PayBackRate,
								 PayBackRateWholesale = p.PayBackRateWholesale,
								 PurchasePrice = p.PurchasePrice,
								 WholesalePrice = p.WholesalePrice,
								 ImagePaths = (from pi in context.ProductImages
											   where pi.ProductId == p.Id
											   select pi.ImagePath).ToList()
							 };
				return result.FirstOrDefault();

			}
		}
    }
}
