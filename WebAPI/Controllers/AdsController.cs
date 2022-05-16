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
	public class AdsController : ControllerBase
	{
		// IoC Container -- Inversion of Control
		IAdsService _adsService;

		public AdsController(IAdsService adsService)
		{
			_adsService = adsService;
		}

	
		[HttpGet("getall")]
		public IActionResult GetAll()
		{
			var images = _adsService.GetAll();
			if (!images.Success)
			{
				return BadRequest(images);
			}
			return Ok(images);
		}

		[HttpPost("addnewads")]
		public IActionResult AddNewAds([FromForm(Name = ("Image"))] IFormFile file)
		{
			var result = _adsService.Add(file);
			if (!result.Success)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpGet("deleteads")]
		public IActionResult DeleteAds(int adsId)
		{
			var result = _adsService.Remove(adsId);
			if (!result.Success)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

	}
}
