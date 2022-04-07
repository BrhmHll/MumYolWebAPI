using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.IoC;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class ProductManager : IProductService
	{
		IProductDal _productDal;

		public ProductManager(IProductDal productDal)
		{
			_productDal = productDal;
		}

		[SecuredOperation("personnel,admin")]
		[ValidationAspect(typeof(ProductValidator))]
		[CacheRemoveAspect("IProductService.Get")]
		[TransactionScopeAspect]
		public IResult Add(Product product)
		{
			IResult result = BusinessRules.Run(
				//CheckIfProductCountOfCategoryCount(product.CategoryId),
				CheckIfProductNameExists(product.Name));

			if (result != null)
				return result;

			_productDal.Add(product);
			return new SuccessResult(Messages.ProductAdded);

		}

		[SecuredOperation("user,personnel,admin")]
		[CacheAspect]
		public IDataResult<List<Product>> GetAll()
		{
			//İş Kodları
			//if (DateTime.Now.Hour == 5)
			//{
			//	return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
			//}
			return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.IsActive.Equals(true)), Messages.ProductListed);
		}

		[SecuredOperation("user,personnel,admin")]
		[CacheAspect]
		public IDataResult<List<Product>> GetAllByCategoryId(int id)
		{
			return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id && p.IsActive.Equals(true)));
		}

		[SecuredOperation("user,personnel,admin")]
		public IDataResult<List<Product>> GetAllByUnitPrice(decimal min, decimal max)
		{
			return new SuccessDataResult<List<Product>> (_productDal.GetAll(p => p.RetailPrice >= min && p.RetailPrice <= max && p.IsActive.Equals(true)));
		}

		[CacheAspect]
		//[PerformanceAspect(3)]
		[SecuredOperation("user,personnel,admin")]
		public IDataResult<Product> GetById(int productId)
		{
			return new SuccessDataResult<Product>(_productDal.Get(p => p.Id == productId));
		}

		[SecuredOperation("user,personnel,admin")]
		public IResult SetActive(ProductActiveDto productActive)
        {
			var product = GetById(productActive.ProductId);
			if (!product.Success)
				return product;

			product.Data.IsActive = productActive.IsActive;

			var res = Update(product.Data);
            if (!res.Success)
				return res;

			return new SuccessResult();
        }

        [SecuredOperation("personnel,admin")]
		[ValidationAspect(typeof(ProductValidator))]
		[CacheRemoveAspect("IProductService.Get")]
		public IResult Update(Product product)
		{
			_productDal.Update(product);
			return new SuccessResult();
		}


		private IResult CheckIfProductCountOfCategoryCount(int categoryId)
		{
			var productCount = _productDal.GetAll(p => p.CategoryId ==categoryId).Count();
			if (productCount >= 10)
				return new ErrorResult(Messages.TooManyProductInThisCategory);

			return new SuccessResult();
		}

		private IResult CheckIfProductNameExists(string productName)
		{
			var result = _productDal.GetAll(p => p.Name == productName).Any();
			if (result)
				return new ErrorResult(Messages.ProductNameAlreadyExists);

			return new SuccessResult();
		}

	}
}
