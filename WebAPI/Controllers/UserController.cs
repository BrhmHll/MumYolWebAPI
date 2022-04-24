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
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		// IoC Container -- Inversion of Control
		IUserService _userService;
        IBalanceHistoryService _balanceHistoryService;

		public UserController(IUserService userService, IBalanceHistoryService balanceHistoryService)
		{
			_userService = userService;
            _balanceHistoryService = balanceHistoryService;
		}

        [HttpGet("getuserprofile")]
        public IActionResult GetUserProfile()
        {
            var result = _userService.GetUserProfile();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getalluserprofile")]
        public IActionResult GetAllUserProfile()
        {
            var result = _userService.GetAllUserProfile();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("updateuserprofile")]
        public IActionResult UpdateUserProfile(UserForUpdateDto user)
        {
            var result = _userService.UpdateUserProfile(user);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbalancehistory")]
        public IActionResult GetBalanceHistory()
        {
            var result = _balanceHistoryService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //[HttpGet("getallbyiswholesale")]
        //public IActionResult GetAllByIsWholesale(int topCategoryId)
        //{
        //	//Swagger
        //	var result = _categoryService.GetAllByIsWholesale(topCategoryId);
        //	if (result.Success)
        //	{
        //		return Ok(result);
        //	}
        //	return BadRequest(result);
        //}



        //[HttpPost("add")]
        //public IActionResult Add(Category category)
        //{
        //	var result = _categoryService.Add(category);
        //	if (result.Success)
        //	{
        //		return Ok(result);
        //	}
        //	return BadRequest(result);
        //}




    }
}
