using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TaskManagerBackend.DTO;
using TaskManagerBackend.Errors;
using TaskManagerBackend.Models;
using TaskManagerBackend.Services;

namespace TaskManagerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly OrganizationServices organizationServices;

        public OrganizationController(OrganizationServices organizationServices)
        {
            this.organizationServices = organizationServices;
        }

        [HttpGet("GetAllOrganizationDetails")]
        public async Task<ApiResponse<List<OrganizationSchema>>> GetAllOrganizationDetails()
        {
            return await organizationServices.GetAllOrganizationDetailsAsync();
        }


        [HttpGet("GetOrganizationDetails")]
        public async Task<ApiResponse<OrganizationSchema>> GetOrganizationDetails(string organizationId)
        {
            return await organizationServices.GetOrganaizationDetailsAsync(organizationId);
        }


        [HttpPost("RegisterOrganization")]
        public async Task<ApiResponse<OrganizationCreationDetaisDTO>> RegisterOrganization(OrganizationSchema schema)
        {
            ApiResponse<OrganizationCreationDetaisDTO> response = new();
            
            if (string.IsNullOrEmpty(schema.Name) || string.IsNullOrEmpty(schema.Description) || string.IsNullOrEmpty(schema.Password))
            {
                response.StatusCode = 400;
                response.Message = "Enter all the fields";

                return response;
            }

            return await organizationServices.RegisterOrganizationAsync(schema);
        }
    }
}
