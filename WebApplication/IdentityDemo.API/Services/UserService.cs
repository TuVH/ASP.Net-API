using IdentityDemo.API.Response;
using IdentityDemo.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityDemo.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        public UserService(UserManager<IdentityUser> userManager, 
            IConfiguration configuration, IMailService mailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mailService = mailService;
        }

        

        public async Task<UserManagerResponse> LoginUser(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no Email Exist",
                    Issuccess = false
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.PassWord);
            if (!result)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid Password",
                    Issuccess = false
                };
            }
            var claims = new[]
            {
                new Claim("Email",model.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMonths(6),
                signingCredentials: new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256Signature)
                );
            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = tokenAsString,
                Issuccess = true ,
                ExpireDate = token.ValidTo
                
            };
        }

        public async Task<UserManagerResponse> RegisterUser(RegisterViewModel model)
        {
            if (model is null)
            {
                throw new NullReferenceException("Model is null");
            }
            if (model.PassWord != model.ConfirmPassWord)
            {
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password",
                    Issuccess = false
                };
            }
            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(identityUser, model.PassWord);
            if (result.Succeeded)
            {
                //Genarate Token when Register
                var generateEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                var byteEmail = Encoding.UTF8.GetBytes(generateEmailToken);
                var tokenEncode = WebEncoders.Base64UrlEncode(byteEmail);

                string url =
                    $"{_configuration["AppUrl"]}/api/auth/confirmemail?id={identityUser.Id}&token={tokenEncode}";

                await _mailService.SendMail(identityUser.Email, "Email Confirm", 
                    $"<h1>Welcome to my application</h1>"+
                    $"<p>Please confirm your Email by <a href='{url}'> Clicking here </a> </p>");

                return new UserManagerResponse
                {
                    Message = " successful",
                    Issuccess = true,
                };
            }
            return new UserManagerResponse
            {
                Message = "Didnot create successful",
                Issuccess = false,
                Errors = result.Errors.Select(x => x.Description)

            };
            

        }
        public async Task<UserManagerResponse> EmailConfirm(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return new UserManagerResponse
                {
                    Issuccess = false,
                    Message = "Cannot find your User"
                };
            }
            var tokenDecode = WebEncoders.Base64UrlDecode(token);
            var normalToken = Encoding.UTF8.GetString(tokenDecode);

            var result = await _userManager.ConfirmEmailAsync(user, normalToken);
            if (!result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Issuccess = false,
                    Message = "Cannot find your User"
                };
            }
            return new UserManagerResponse
            {
                Issuccess = true,
                Message = "Successful Confirm"
            };

        }
    }
}
