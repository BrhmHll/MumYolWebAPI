using Business.Abstract;
using Business.Concrete;
using Core.Utilities.Results;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		// IoC Container -- Inversion of Control
		IProductService _productService;
		
		public ProductsController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet("getall")]
		public IActionResult GetAll()
		{
			//Swagger
			var result = _productService.GetAll();
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpGet("getbyid")]
		public IActionResult GetDetailsById(int id)
		{
			var result = _productService.GetDetailsById(id);
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpGet("getbyid2")]
		public IActionResult GetDetailsById2(int id)
		{
			var result = _productService.GetDetailsById(id);
			IDataResult<List<ProductDetailDto>> newResult;
			
			if (result.Success)
			{
				newResult = new SuccessDataResult<List<ProductDetailDto>>(new List<ProductDetailDto>());
				newResult.Data.Add(result.Data);
				return Ok(newResult);
			}
			newResult = new ErrorDataResult<List<ProductDetailDto>>();
			return BadRequest(newResult);
		}

		[HttpPost("add")]
		public IActionResult Add(Product product)
		{
			var result = _productService.Add(product);
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpPost("update")]
		public IActionResult Update(Product product)
		{
			var result = _productService.Update(product);
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpGet("getbycategoryid")]
		public IActionResult GetByCategoryId(int categoryid)
		{
			var result = _productService.GetAllByCategoryId(categoryid);
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpGet("getproductdetailsbycategoryid")]
		public IActionResult GetProductDetailsByCategoryId(int categoryid)
		{
			var result = _productService.GetAllProductDetailsByCategoryId(categoryid);
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpPost("setactive")]
		public IActionResult SetActive(ProductActiveDto productActiveDto)
		{
			var result = _productService.SetActive(productActiveDto);
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}


		[HttpGet("searchall")]
		public IActionResult SearchAll(string searchKey)
		{
			var result = _productService.SearchAll(searchKey);
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}
	}
}
