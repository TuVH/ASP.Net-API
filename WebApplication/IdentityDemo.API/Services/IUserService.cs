using IdentityDemo.API.Response;
using IdentityDemo.API.ViewModels;
using System.Threading.Tasks;

namespace IdentityDemo.API.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUser(RegisterViewModel model);

        Task<UserManagerResponse> LoginUser(LoginViewModel model);
    }
}
