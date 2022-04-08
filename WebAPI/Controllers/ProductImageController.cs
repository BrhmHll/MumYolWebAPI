using Business.Abstract;
using Business.Concrete;
using Business.Constants;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductImageController : ControllerBase
	{
		// IoC Container -- Inversion of Control
		IProductImageService _productImageService;

		public ProductImageController(IProductImageService productImageService)
		{
			_productImageService = productImageService;
		}

		[HttpPost("addnewproductimagelink")]
		public IActionResult AddNewProductImageLink([FromForm] ProductImage productImage)
		{
			var result = _productImageService.AddImageLink(productImage);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}
			return Ok(Messages.Success);
		}

		[HttpPost("updateproductimagelink")]
		public IActionResult UpdateProductImageLink([FromForm(Name = ("Image"))] IFormFile file, [FromForm(Name = ("Id"))] int id)
		{
			var productImageResult = _productImageService.Get(id);
			var result = _productImageService.UpdateImageLink(productImageResult.Data);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}
			return Ok(Messages.Success);
		}

		[HttpPost("deleteproductimagelink")]
		public IActionResult DeleteProductImageLink(ProductImage productImage)
		{
			var result = _productImageService.DeleteImageLink(productImage);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}
			return Ok(Messages.Success);
		}


		[HttpGet("getallimagesbyproductid")]
		public IActionResult GetAllProductImagesByProductId(int productId)
		{
			var images = _productImageService.GetAllImagesByProductId(productId);
			if (!images.Success)
			{
				return BadRequest(images);
			}
			return Ok(images);
		}

		[HttpGet("getimagebyproductid")]
		public IActionResult GetProductImageByProductId(int productId)
		{
			var image = _productImageService.GetImageByProductId(productId);
			if (!image.Success)
			{
				return BadRequest(image);
			}
			return Ok(image);
		}

		[HttpPost("addnewproductimage")]
		public IActionResult AddNewProductImage([FromForm(Name = ("Image"))] IFormFile file, [FromForm] ProductImage productImage)
		{
			var result = _productImageService.AddImage(file, productImage);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}
			return Ok(Messages.Success);
		}

		[HttpPost("updateproductimage")]
		public IActionResult UpdateProductImage([FromForm(Name = ("Image"))] IFormFile file, [FromForm(Name = ("Id"))] int id)
		{
			var productImageResult = _productImageService.Get(id);
			var result = _productImageService.UpdateImage(file, productImageResult.Data);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}
			return Ok(Messages.Success);
		}

		[HttpPost("deleteproductimage")]
		public IActionResult DeleteProductImage(ProductImage productImage)
		{
			var result = _productImageService.DeleteImage(productImage);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}
			return Ok(Messages.Success);
		}



	}
}
