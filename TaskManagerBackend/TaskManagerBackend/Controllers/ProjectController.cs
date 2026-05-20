using Microsoft.AspNetCore.Mvc;
using TaskManagerBackend.DTO;
using TaskManagerBackend.Errors;
using TaskManagerBackend.Models;
using TaskManagerBackend.Services;

namespace TaskManagerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectServices projectServices;

        public ProjectController(ProjectServices projectServices)
        {
            this.projectServices = projectServices;
        }


        [HttpGet("GetAllProjectDetails")]
        public async Task<ApiResponse<List<ProjectSchema>>> GetAllProjectDeatails(string email)
        {
            return await  projectServices.GetAllProjectDetailsAsync(email);
        }


        [HttpGet("GetProjectDetails")]
        public async Task<ApiResponse<ProjectSchema>> GetProjectDetails(string projectId)
        {
            return await projectServices.GetProjectDetailsAsync(projectId);
        }


        [HttpPost("CreateProject")]
        public async Task<ApiResponse<ProjectCreationDetailsDTO>> CreateProject(ProjectSchema prj)
        {
            ApiResponse<ProjectCreationDetailsDTO> response = new();
            
            if(string.IsNullOrEmpty(prj.ProjectName) || string.IsNullOrEmpty(prj.AdminEmail) || prj.ProjectMembers.Length == 0)
            {
                response.StatusCode = 400;
                response.Message = "Enter All The Fields";

                return response;
            }

            return await projectServices.CreateNewProjectAsync(prj);
        }
    }
}
