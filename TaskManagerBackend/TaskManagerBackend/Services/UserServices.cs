using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskManagerBackend.DTO;
using TaskManagerBackend.Errors;
using TaskManagerBackend.Models;

namespace TaskManagerBackend.Services
{
    public class UserServices
    {
        private readonly IMongoCollection<UserSchema> userSchema;
        private readonly JWTservices jWTservices;

        public UserServices(IOptions<DatabaseSettings> databaseSettings, JWTservices jWTservices)
        {
            MongoClient mongo = new MongoClient(databaseSettings.Value.DatabaseConnectionString);
            userSchema = mongo.GetDatabase(databaseSettings.Value.DatabaseName).GetCollection<UserSchema>(databaseSettings.Value.UserCollectionName);
            this.jWTservices = jWTservices;
        }


        // Service to get all the member
        public async Task<List<UserSchema>> GetAllUserInfoAsync(string organizationId)
        {
            return await userSchema.Find(all => all.Role == "Member" && all.OrganizationId == organizationId).ToListAsync();    
        }


        // Service to login user
        public async Task<ApiResponse<UserDetailsDTO>> GetUserLoginInfoAsync(UserLoginDTO loginDTO)
        {
            UserSchema user =  await userSchema.Find(u => u.UserEmail == loginDTO.Email).FirstOrDefaultAsync();

            ApiResponse<UserDetailsDTO> response = new();
            response.Data = new UserDetailsDTO();

            if (user == null)
            {
                response.StatusCode = 404;
                response.Message = "User not found please check";

                return response;
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.UserPassword))
            {
                response.StatusCode = 400;
                response.Message = "Invalid email or password";

                return response;
            }

            

            response.StatusCode = 200;
            response.Message = "User found";

            response.Data.UserName = user.UserName;
            response.Data.UserEmail = user.UserEmail;
            response.Data.Role = user.Role;
            response.Data.OrganizationId = user.OrganizationId;
            response.Data.Token = jWTservices.GenerateToken(user);

            return response;
        } 


        // Service to get user info
        public async Task<ApiResponse<UserDetailsDTO>> GetUserDetailsAsync(string email)
        {
            UserSchema user = await userSchema.Find(us => us.UserEmail == email).FirstOrDefaultAsync();

            ApiResponse<UserDetailsDTO> response = new();
            response.Data = new UserDetailsDTO();

            if (user == null) 
            {
                response.StatusCode = 404;
                response.Message = "User not found please check";

                return response;
            }

            response.StatusCode = 200;
            response.Message = "User found";

            response.Data.UserName = user.UserName;
            response.Data.UserEmail = user.UserEmail;
            response.Data.OrganizationId = user.OrganizationId;
            response.Data.Role = user.Role;

            return response;
        }


        // Service to register user
        public async Task<ApiResponse<UserDetailsDTO>> RegisterUser(UserSchema newUser)
        {
            UserSchema user = await userSchema.Find(u => u.UserEmail == newUser.UserEmail).FirstOrDefaultAsync();

            ApiResponse<UserDetailsDTO> response = new();            
            response.Data = new UserDetailsDTO();

            if (user != null)
            {
                response.StatusCode = 403;
                response.Message = "User already exist";

                response.Data!.UserName = user.UserName;
                response.Data!.UserEmail = user.UserEmail;
                response.Data!.Role = user.Role;
                response.Data!.OrganizationId = user.OrganizationId;

                return response;
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.UserPassword);
            newUser.UserPassword = hashedPassword;

            await userSchema.InsertOneAsync(newUser);

            response.StatusCode = 201;
            response.Message = "User registered";

            response.Data.UserName = newUser.UserName;
            response.Data.UserEmail = newUser.UserEmail;
            response.Data.Role = newUser.Role;
            response.Data.OrganizationId = newUser.OrganizationId;

            return response;
        }
    }
}
