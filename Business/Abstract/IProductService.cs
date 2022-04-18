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
	public interface IProductService
	{
		IDataResult<List<Product>> GetAll();
		IDataResult<List<ProductDetailDto>> SearchAll(string searchKey);
		IDataResult<List<Product>> GetAllByCategoryId(int categoryId);
		IDataResult<List<Product>> GetAllByUnitPrice(decimal min, decimal max);
		IDataResult<List<ProductDetailDto>> GetAllProductDetailsByCategoryId(int categoryId);
		IDataResult<ProductDetailDto> GetById(int productId);
		IResult Add(Product product);
		IResult Update(Product product);
		IResult SetActive(ProductActiveDto productActive);

	}
}
