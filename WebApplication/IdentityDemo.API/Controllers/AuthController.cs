using IdentityDemo.API.Services;
using IdentityDemo.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService,IMailService mailService,
            IConfiguration configuration)
        {
            _userService = userService;
            _mailService = mailService;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUser(model);
                if (result.Issuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("some are not valid"); // status code : 400
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUser(model);
                if (result.Issuccess)
                {
                    //await _mailService.SendMail(model.Email, "New Login", "<h1>Hey! New Login from me</h1>");
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("some are not valid"); // status code : 400
        }
        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string id, string token)
        {
            var result = await _userService.EmailConfirm(id, token);
            if (result.Issuccess)
            {
                return Redirect($"{_configuration["AppUrl"]}/Index.html");
            }
            return BadRequest(result);
        }
        [HttpPost("forgetpassword")]
        public async Task<IActionResult> Forgetpassword(string email)
        {
            var result = await _userService.ForgetPassword(email);
            if (result.Issuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm]ResetPasswordViewModel reset)
        {
            var result = await _userService.ResetPassword(reset);
            if (result.Issuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        // GET: api/<AuthController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AuthController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuthController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuthController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
