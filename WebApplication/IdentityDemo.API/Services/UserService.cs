using IdentityDemo.API.Response;
using IdentityDemo.API.ViewModels;
using Microsoft.AspNetCore.Identity;
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
        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
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
    }
}
