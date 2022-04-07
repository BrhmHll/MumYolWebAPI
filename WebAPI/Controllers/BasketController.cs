using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BasketController : ControllerBase
	{
		IBasketService _basketService;
		
		public BasketController(IBasketService basketService)
		{
			_basketService = basketService;
		}

		[HttpGet("getall")]
		public IActionResult GetAll()
		{
			var result = _basketService.GetAll();
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpPost("add")]
		public IActionResult Add(BasketDto basketDto)
		{
			var result = _basketService.Add(basketDto);
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpPost("update")]
		public IActionResult Update(BasketItem basketItem)
		{
			var result = _basketService.Update(basketItem);
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpGet("remove")]
		public IActionResult Remove(int id)
		{
			var result = _basketService.Delete(id);
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpGet("removeall")]
		public IActionResult RemoveAll()
		{
			var result = _basketService.DeleteAll();
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}



	}
}
