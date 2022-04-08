using Business.Abstract;
using Business.Constants;
using Core.Utilities.Integrations;
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
    public class AuthController : Controller
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult Login(UserForLoginDto panelUserForLoginDto)
        {
            var userToLogin = _authService.Login(panelUserForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("register")]
        public ActionResult Register(UserForRegisterDto panelUserForRegisterDto)
        {
            var userExists = _authService.UserExists(panelUserForRegisterDto.PhoneNumber);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _authService.Register(panelUserForRegisterDto, panelUserForRegisterDto.Password);
            var result = _authService.CreateAccessToken(registerResult.Data);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("sendsms")]
        public ActionResult SendSms(string phone)
        {
            var code = SmsIntegration.GenerateCode();
            var msg = string.Format(Messages.VerificationMessage, "Ibrahim Halil SAKAR", code);
            var res = SmsIntegration.SendSms(phone, msg);
            return Ok(res);

        }
    }
}
