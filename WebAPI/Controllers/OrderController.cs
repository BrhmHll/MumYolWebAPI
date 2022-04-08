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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		// IoC Container -- Inversion of Control
		IOrderService _orderService;

		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		[HttpGet("getallordersbyuser")]
		public IActionResult GetAllOrdersByUser()
		{
			//Swagger
			var result = _orderService.GetOrdersByUser();
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpPost("orderbasket")]
		public IActionResult OrderBasket()
		{
			var result = _orderService.OrderBasket();
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpGet("getorderdetailsbyid")]
		public IActionResult GetOrderDetailsById(int orderId)
		{
			//Swagger
			var result = _orderService.GetOrderDetailsById(orderId);
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpPost("updateorderstatus")]
		public IActionResult UpdateOrderStatus(UpdateOrderStatusDto updateOrderStatusDto)
		{
			var result = _orderService.UpdateOrderStatus(updateOrderStatusDto);
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpGet("getallordersbystatusid")]
		public IActionResult GetAllOrdersByStatusId(int statusId)
		{
			//Swagger
			var result = _orderService.GetAllOrdersByStatusId(statusId);
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}
	}
}
