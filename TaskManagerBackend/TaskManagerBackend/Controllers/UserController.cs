using Microsoft.AspNetCore.Mvc;
using TaskManagerBackend.DTO;
using TaskManagerBackend.Errors;
using TaskManagerBackend.Models;
using TaskManagerBackend.Services;

namespace TaskManagerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserServices userServices;

        public UserController(UserServices userServices)
        {
            this.userServices = userServices;
        }


        [HttpGet("GetAllUser")]
        public async Task<List<UserSchema>> GetAllUserAsync(string organizationId)
        {
            return await userServices.GetAllUserInfoAsync(organizationId);
        }


        [HttpGet("GetUser")]
        public async Task<ApiResponse<UserDetailsDTO>> GetUserasync(string email)
        {
            ApiResponse<UserDetailsDTO> response = new();

            if (string.IsNullOrEmpty(email))
            {
                response.StatusCode = 400;
                response.Message = "Please enter all the fields";

                return response;
            }

            return await userServices.GetUserDetailsAsync(email);
        }


        [HttpPost("Login")]
        public async Task<ApiResponse<UserDetailsDTO>> LoginUser(UserLoginDTO user)
        {
            var response = new ApiResponse<UserDetailsDTO>();

            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password)) 
            {
                response.StatusCode = 400;
                response.Message = "Please enter all the fields";

                return response;
            }

            return await userServices.GetUserLoginInfoAsync(user);
        }


        [HttpPost("Register")]
        public async Task<ApiResponse<UserDetailsDTO>> RegisterUser(UserSchema user)
        {
            var response = new ApiResponse<UserDetailsDTO>();

            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.UserEmail) || string.IsNullOrEmpty(user.UserPassword) || string.IsNullOrEmpty(user.Role))
            {
                response.StatusCode = 400;
                response.Message = "Please enter all the fields";

                return response;
            }

            return await userServices.RegisterUser(user);
        }
    }
}
