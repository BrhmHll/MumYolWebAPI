using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IProductImageService
    {
		public IResult AddImage(IFormFile file, int productId);
		public IResult UpdateImage(IFormFile file, ProductImage productImage);
		public IResult DeleteImageById(int id);
		public IResult DeleteAllImagesByProductId(int productId);
		//public IResult AddImageLink(ProductImage productImage);
		//public IResult UpdateImageLink(ProductImage productImage);
		public IDataResult<List<ProductImage>> GetAllImagesByProductId(int productId);
		public IDataResult<ProductImage> GetImageByProductId(int productId);
		public IDataResult<ProductImage> Get(int id);
	}
}
