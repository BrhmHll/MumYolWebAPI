using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Helper;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductImageManager : IProductImageService
    {
        IProductImageDal _productImageDal;
		IProductService _productService;

		private string logoPath = "https://cdn.logo.com/hotlink-ok/logo-social.png";

		public ProductImageManager(IProductImageDal ProductImageDal, IProductService productService)
        {
            _productImageDal = ProductImageDal;
			_productService = productService;
        }

		[SecuredOperation("admin,personel")]
		public IResult AddImageLink(ProductImage productImage)
		{
			var countOfImage = _productImageDal.GetAll(i => i.ProductId == productImage.ProductId).Count();
			if (countOfImage > 4)
			{
				return new ErrorResult("En fazla 4 resim yuklenebilir");
			}

			var carIsExists = _productService.GetById(productImage.ProductId);
			if (!carIsExists.Success)
			{
				return new ErrorResult("Urun bulunamadi!");
			}
			productImage.Date = DateTime.Now;
			_productImageDal.Add(productImage);
			return new SuccessResult(Messages.Success);
		}

		[SecuredOperation("admin,personel")]
		public IResult UpdateImageLink(ProductImage productImage)
		{
			var isImage = _productImageDal.Get(c => c.Id == productImage.Id);
			if (isImage == null)
			{
				return new ErrorResult("Resim bulunamadi!");
			}

			isImage.Date = DateTime.Now;
			isImage.ImagePath = productImage.ImagePath;
			_productImageDal.Update(productImage);
			return new SuccessResult(Messages.Success);
		}

		[SecuredOperation("admin,personel")]
		public IResult DeleteImageLink(ProductImage productImage)
		{
			var image = _productImageDal.Get(c => c.Id == productImage.Id);
			if (image == null)
			{
				return new ErrorResult("Resim bulunamadi!");
			}

			_productImageDal.Delete(productImage);
			return new SuccessResult(Messages.Success);
		}

		[SecuredOperation("admin,personel")]
		public IResult AddImage(IFormFile file, ProductImage productImage)
		{
			var countOfImage = _productImageDal.GetAll(i => i.ProductId == productImage.ProductId).Count();
			if (countOfImage > 4)
			{
				return new ErrorResult("En fazla 4 resim yuklenebilir");
			}

			var carIsExists = _productService.GetById(productImage.ProductId);
			if (!carIsExists.Success)
			{
				return new ErrorResult("Urun buluanamadi!");
			}

			var imageResult = FileHelper.Upload(file);
			if (!imageResult.Success)
			{
				return new ErrorResult(imageResult.Message);
			}

			productImage.ImagePath = imageResult.Message;
			productImage.Date = DateTime.Now;
			productImage.Id = default;
			_productImageDal.Add(productImage);
			return new SuccessResult(Messages.Success);
		}
		[SecuredOperation("admin,personel")]
		public IResult DeleteImage(ProductImage productImage)
		{
			var image = _productImageDal.Get(c => c.Id == productImage.Id);
			if (image == null)
			{
				return new ErrorResult("Hatali deger!");
			}

			FileHelper.Delete(image.ImagePath);
			_productImageDal.Delete(productImage);
			return new SuccessResult(Messages.Success);
		}
		[SecuredOperation("admin,personel")]
		public IDataResult<ProductImage> Get(int id)
		{
			var image = _productImageDal.Get(i => i.Id == id);
			if (image == null)
			{
				return new ErrorDataResult<ProductImage>("Hatali deger!");
			}
			return new SuccessDataResult<ProductImage>();
		}
		[SecuredOperation("admin,personel")]
		public IDataResult<List<ProductImage>> GetAllImagesByProductId(int carId)
		{
			var images = _productImageDal.GetAll(i => i.ProductId == carId);
			if (images.Count == 0)
			{
				var carIsExists = _productService.GetById(carId);
				if (!carIsExists.Success)
				{
					return new ErrorDataResult<List<ProductImage>>("Hatali deger!");
				}
				ProductImage productImage = new ProductImage();
				productImage.ProductId = carId;
				productImage.Date = DateTime.Now;
				productImage.ImagePath = logoPath;
				return new SuccessDataResult<List<ProductImage>>(new List<ProductImage> { productImage });
			}
			return new SuccessDataResult<List<ProductImage>>(images);

		}
		[SecuredOperation("admin,personel")]
		public IDataResult<ProductImage> GetImageByProductId(int carId)
		{
			var result = GetAllImagesByProductId(carId);
			if (!result.Success)
			{
				return new ErrorDataResult<ProductImage>(result.Message);
			}
			return new SuccessDataResult<ProductImage>(result.Data[0]);
		}

		[SecuredOperation("admin,personel")]
		public IResult UpdateImage(IFormFile file, ProductImage productImage)
		{
			var isImage = _productImageDal.Get(c => c.Id == productImage.Id);
			if (isImage == null)
			{
				return new ErrorResult("Hatali deger!");
			}

			var updatedFile = FileHelper.Update(file, isImage.ImagePath);
			if (!updatedFile.Success)
			{
				return new ErrorResult(updatedFile.Message);
			}
			productImage.ImagePath = updatedFile.Message;

			productImage.Date = DateTime.Now;

			_productImageDal.Update(productImage);
			return new SuccessResult(Messages.Success);
		}

	}
}
