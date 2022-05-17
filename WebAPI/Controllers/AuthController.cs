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
                return BadRequest(userToLogin);

            var res = _authService.CreateAccessToken(userToLogin.Data);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("register")]
        public ActionResult Register(UserForRegisterDto panelUserForRegisterDto)
        {
            var userExists = _authService.UserExists(panelUserForRegisterDto.PhoneNumber);
            if (!userExists.Success)
                return BadRequest(userExists);

            var registerResult = _authService.Register(panelUserForRegisterDto);
            if (!registerResult.Success)
                return BadRequest(registerResult);

            var result = _authService.CreateAccessToken(registerResult.Data);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpGet("registerphone")]
        public ActionResult RegisterPhone(string phoneNumber)
        {
            var userExists = _authService.UserExists(phoneNumber);
            if (!userExists.Success)
                return BadRequest(userExists);

            var res = _authService.RegisterPhone(phoneNumber);
            if (!res.Success)
                return BadRequest(res);

            return Ok(res);
        }

        [HttpGet("checkphone")]
        public ActionResult CheckPhone(string phoneNumber, string verificationCode)
        {
            var res = _authService.CheckPhone(phoneNumber, verificationCode);
            if (!res.Success)
                return BadRequest(res);

            return Ok(res);
        }

        [HttpGet("resetpasswordbyadmin")]
        public ActionResult ResetPasswordByAdmin(int userId)
        {
            var res = _authService.ResetPasswordByAdmin(userId);
            if (!res.Success)
                return BadRequest(res);

            return Ok(res);
        }

        [HttpPost("sendsms")]
        public ActionResult SendSms(string phone)
        {
            var code = SmsIntegration.GenerateCode();
            var msg = string.Format(Messages.VerificationMessage, "Ibrahim Halil SAKAR", code);
            var res = SmsIntegration.SendSms(phone, msg);
            return Ok(res);
        }

        [HttpGet("apitest")]
        public ActionResult ApiTest()
        {
            return Ok("{\"message\" : \"Hello World! This is API Test.\"}");
        }

        [HttpGet("getlastuser")]
        public ActionResult LastUser()
        {
            return Ok(_authService.GetLastRegisteredUser());
        }
    }
}
