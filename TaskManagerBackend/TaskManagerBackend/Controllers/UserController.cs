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
        private readonly JWTservices jwtServices;

        public UserController(UserServices userServices, JWTservices jwtServices)
        {
            this.userServices = userServices;
            this.jwtServices = jwtServices;
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
            response.Data = new UserDetailsDTO();

            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password)) 
            {
                response.StatusCode = 400;
                response.Message = "Please enter all the fields";

                return response;
            }

            var existUser = await userServices.GetUserLoginInfoAsync(user);

            response.StatusCode = existUser.StatusCode;
            response.Message = existUser.Message;

            if (existUser.Data != null && existUser.Data.Id != null && existUser.Data.UserEmail != null)
            {
                response.Data.UserEmail = existUser.Data.UserEmail;
                response.Data.UserName = existUser.Data.UserName;

                var token = jwtServices.GenerateToken(existUser.Data.Id, existUser.Data.UserEmail);

                Response.Cookies.Append("accessToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(1)
                });
            }

            return response;
        }


        [HttpPost("Register")]
        public async Task<ApiResponse<UserDetailsDTO>> RegisterUser(UserSchema user)
        {
            var response = new ApiResponse<UserDetailsDTO>();

            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.UserEmail) || string.IsNullOrEmpty(user.UserPassword) )
            {
                response.StatusCode = 400;
                response.Message = "Please enter all the fields";

                return response;
            }

            response = await userServices.RegisterUser(user);


            return response;
        }
    }
}
