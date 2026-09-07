using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerBackend.DTO;
using TaskManagerBackend.Errors;
using TaskManagerBackend.Models;
using TaskManagerBackend.Services;

namespace TaskManagerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectServices projectServices;

        public ProjectController(ProjectServices projectServices)
        {
            this.projectServices = projectServices;
        }


        [HttpGet("GetAllProjectDetails")]
        public async Task<ApiResponse<List<ProjectSchema>>> GetAllProjectDeatails()
        {
            string userEmail = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return await  projectServices.GetAllProjectDetailsAsync(userEmail);
        }


        [HttpGet("GetProjectDetails")]
        public async Task<ApiResponse<ProjectSchema>> GetProjectDetails(string projectId)
        {
            return await projectServices.GetProjectDetailsAsync(projectId);
        }


        [HttpPost("CreateProject")]
        public async Task<ApiResponse<ProjectCreationDetailsDTO>> CreateProject(ProjectCreationDetailsDTO prj)
        {
            ApiResponse<ProjectCreationDetailsDTO> response = new();

            if(string.IsNullOrEmpty(prj.ProjectName))
            {
                response.StatusCode = 400;
                response.Message = "Enter All The Fields";

                return response;
            }

            string userEmail = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            
            ProjectSchema projectSchema = new ProjectSchema();

            projectSchema.ProjectName = prj.ProjectName;
            projectSchema.UserEmail = userEmail;

            return await projectServices.CreateNewProjectAsync(projectSchema);
        }


        [HttpPost("Update")]
        public async Task<ApiResponse<string>> UpdateProject(ProjectUpdationDTO project)
        {
            return await projectServices.UpdateProject(project);
        }


        [HttpPost("Delete")]
        public async Task<ApiResponse<string>> DeleteProject(string projectId)
        {
            return await projectServices.DeleteProject(projectId);
        }
    }
}
