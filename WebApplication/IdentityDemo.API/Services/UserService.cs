using IdentityDemo.API.Response;
using IdentityDemo.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.API.Services
{
    public class UserService : IUserService
    {
        private UserManager<IdentityUser> _userManager;
        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
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
