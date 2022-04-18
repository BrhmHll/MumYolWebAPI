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
		ICategoryService _categoryService;

		public ProductManager(IProductDal productDal, ICategoryService categoryService)
		{
			_productDal = productDal;
			_categoryService = categoryService;
		}

		[SecuredOperation("personnel,admin")]
		[ValidationAspect(typeof(ProductValidator))]
		[CacheRemoveAspect("IProductService.Get")]
		[TransactionScopeAspect]
		public IResult Add(Product product)
		{
			IResult result = BusinessRules.Run(
				CheckIfCategoryExists(product.CategoryId),
				CheckIfProductNameExists(product.Name));
			if (result != null)
				return result;

			_productDal.Add(product);
			CheckCategory(product.CategoryId);

			return new SuccessResult(Messages.ProductAdded);

		}

		[SecuredOperation("user,personnel,admin")]
		[CacheAspect]
		public IDataResult<List<Product>> GetAll()
		{
			return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.IsActive.Equals(true)), Messages.ProductListed);
		}

		[SecuredOperation("user,personnel,admin")]
		[CacheAspect]
		public IDataResult<List<Product>> GetAllByCategoryId(int categoryId)
		{
			return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == categoryId && p.IsActive.Equals(true)));
		}

		[SecuredOperation("user,personnel,admin")]
		public IDataResult<List<Product>> GetAllByUnitPrice(decimal min, decimal max)
		{
			return new SuccessDataResult<List<Product>> (_productDal.GetAll(p => p.RetailPrice >= min && p.RetailPrice <= max && p.IsActive.Equals(true)));
		}

		[SecuredOperation("user,personnel,admin")]
		[CacheAspect]
		public IDataResult<List<ProductDetailDto>> GetAllProductDetailsByCategoryId(int categoryId)
        {
			var productDetailDtos = _productDal.GetAllProductDetails(categoryId);

            foreach (var item in productDetailDtos)
                if (item.ImagePaths.Count == 0)
					item.ImagePaths.Add(ConstantValues.logoPath);

			return new SuccessDataResult<List<ProductDetailDto>>(productDetailDtos);
        }

        [CacheAspect]
		[SecuredOperation("user,personnel,admin")]
		public IDataResult<ProductDetailDto> GetById(int productId)
		{
			return new SuccessDataResult<ProductDetailDto>(_productDal.GetProductDetails(productId));
		}

		[PerformanceAspect(1)]
		public IDataResult<List<ProductDetailDto>> SearchAll(string searchKey)
        {
            if (searchKey == null || searchKey.Length < 3)
				return new ErrorDataResult<List<ProductDetailDto>>("Arama anahtarı 3 karakter veya daha uzun olmalı!");
			var allData = GetAllProductDetailsByCategoryId(0);
			var foundedProducts = new List<ProductDetailDto>();

			foundedProducts.AddRange(allData.Data.Where(p => p.Name.ToLower().Equals(searchKey)));
			foundedProducts.AddRange(allData.Data.Where(p => p.Brand.ToLower().Equals(searchKey)));
			
			if (foundedProducts.Count < ConstantValues.foundedProductCount)
				foundedProducts.AddRange(allData.Data.Where(p => p.Name.ToLower().StartsWith(searchKey)));

			if (foundedProducts.Count < ConstantValues.foundedProductCount)
				foundedProducts.AddRange(allData.Data.Where(p => p.Name.ToLower().Contains(searchKey)));

			var keys = searchKey.Split(' ');

			if (foundedProducts.Count < ConstantValues.foundedProductCount)
				foreach (var key in keys)
					foundedProducts.AddRange(allData.Data.Where(p => p.Brand.ToLower().Equals(key)));

			if (foundedProducts.Count < ConstantValues.foundedProductCount)
				foreach (var key in keys)
					foundedProducts.AddRange(allData.Data.Where(p => p.Brand.ToLower().Contains(key)));

			if (foundedProducts.Count < ConstantValues.foundedProductCount)
				foreach (var key in keys)
					foundedProducts.AddRange(allData.Data.Where(p => p.Name.ToLower().Equals(key)));

			if (foundedProducts.Count < ConstantValues.foundedProductCount)
				foreach (var key in keys)
					foundedProducts.AddRange(allData.Data.Where(p => p.Name.ToLower().StartsWith(key)));

			if (foundedProducts.Count < ConstantValues.foundedProductCount)
				foreach (var key in keys)
					foundedProducts.AddRange(allData.Data.Where(p => p.Name.ToLower().Contains(key)));

			if (foundedProducts.Count < ConstantValues.foundedProductCount)
				foreach (var key in keys)
					foundedProducts.AddRange(allData.Data.Where(p => p.Description.ToLower().Contains(key)));

			foundedProducts = foundedProducts.GroupBy(p => p.Id).Select(p => p.First()).ToList();

			return new SuccessDataResult<List<ProductDetailDto>>(foundedProducts);
        }

        [SecuredOperation("personnel,admin")]
		[CacheRemoveAspect("IProductService.Get")]
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
			CheckCategory(product.CategoryId);
			return new SuccessResult();
		}

		private IResult CheckCategory(int categoryId)
        {
			var cat = _categoryService.GetById(categoryId).Data;
			var products = GetAllByCategoryId(categoryId);
			if (products.Data.Count == 0)
            {
				cat.TopCategoryId = 0;
				_categoryService.Update(cat);
				return new SuccessResult();
            }

			var wholesaleCount = 0;
			var retailCount = 0;
            foreach (var item in products.Data)
            {
                if (item.RetailPrice != 0)
					retailCount += 1;
				if (item.WholesalePrice != 0)							  
					wholesaleCount += 1;
			}
			
			if (retailCount > 0 && wholesaleCount > 0)
				cat.TopCategoryId = 3;
			else if(retailCount > 0)
				cat.TopCategoryId = 2;
			else
				cat.TopCategoryId = 1;

			_categoryService.Update(cat);

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

		private IResult CheckIfCategoryExists(int categoryId)
		{
			var result = _categoryService.GetById(categoryId);
			if (!result.Success)
				return new ErrorResult(Messages.ProductNameAlreadyExists);

			return new SuccessResult();
		}
	}
}
