using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskManagerBackend.DTO;
using TaskManagerBackend.Errors;
using TaskManagerBackend.Models;

namespace TaskManagerBackend.Services
{
    public class OrganizationServices
    {
        private readonly IMongoCollection<OrganizationSchema> organizationSchema;

        public OrganizationServices(IOptions<DatabaseSettings> databaseSettings)
        {
            MongoClient mongo = new MongoClient(databaseSettings.Value.DatabaseConnectionString);
            organizationSchema = mongo.GetDatabase(databaseSettings.Value.DatabaseName).GetCollection<OrganizationSchema>(databaseSettings.Value.OrganizationCollectionName);
        }


        // Service to get all the organization
        public async Task<ApiResponse<List<OrganizationSchema>>> GetAllOrganizationDetailsAsync()
        {
            var organizations = await organizationSchema.Find(organization => true).ToListAsync();
            ApiResponse<List<OrganizationSchema>> response = new();

            response.StatusCode = 200;
            response.Message = "All organizations";
            response.Data = organizations;

            return response;
        }


        // Service to get an organization
        public async Task<ApiResponse<OrganizationSchema>> GetOrganaizationDetailsAsync(string id)
        {
            var organization = await organizationSchema.Find(org => org.Id == id).FirstOrDefaultAsync();
            ApiResponse<OrganizationSchema> response = new();

            if(organization ==  null)
            {
                response.StatusCode = 404;
                response.Message = "Organization not found";

                return response;
            }

            response.StatusCode = 200;
            response.Message = "Organization found";
            response.Data = organization;

            return response;
        }


        // Service to register organization
        public async Task<ApiResponse<OrganizationCreationDetaisDTO>> RegisterOrganizationAsync(OrganizationSchema organization)
        {
            ApiResponse<OrganizationCreationDetaisDTO> response = new();
            response.Data = new OrganizationCreationDetaisDTO();

            var organizationExist = organizationSchema.Find(org => org.Id == organization.Id).FirstOrDefault();
            
            if (organizationExist != null)
            {
                response.StatusCode = 403;
                response.Message = "Organization already exist";
                response.Data.OrganizationName = organization.Name;

                return response;
            }

            var hashPassword = BCrypt.Net.BCrypt.HashPassword(organization.Password);
            organization.Password = hashPassword;

            await organizationSchema.InsertOneAsync(organization);

            response.StatusCode = 201;
            response.Message = "Organization registered";
            response.Data.OrganizationName = organization.Name;

            return response;
        }
    }
}
