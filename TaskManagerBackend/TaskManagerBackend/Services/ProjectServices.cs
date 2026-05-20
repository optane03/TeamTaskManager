using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using TaskManagerBackend.DTO;
using TaskManagerBackend.Errors;
using TaskManagerBackend.Models;

namespace TaskManagerBackend.Services
{
    public class ProjectServices
    {
        private readonly IMongoCollection<ProjectSchema> projectSchema;

        public ProjectServices(IOptions<DatabaseSettings> databaseSettings)
        {
            MongoClient mongo = new MongoClient(databaseSettings.Value.DatabaseConnectionString);
            projectSchema = mongo.GetDatabase(databaseSettings.Value.DatabaseName).GetCollection<ProjectSchema>(databaseSettings.Value.ProjectCollectionName);
        }


        // Service to get all the projects created by a particular admin
        public async Task<ApiResponse<List<ProjectSchema>>> GetAllProjectDetailsAsync(string adminEmail)
        {
            ApiResponse<List<ProjectSchema>> response = new();
            response.Data = new List<ProjectSchema>();

            response.Data = await projectSchema.Find(prj => prj.AdminEmail == adminEmail).ToListAsync();
            response.StatusCode = 200;
            response.Message = "Found All Project";

            return response;
        }


        // Service to get a particullar project details
        public async Task<ApiResponse<ProjectSchema>> GetProjectDetailsAsync(string projectId)
        {
            ProjectSchema prj = await projectSchema.Find(prj => prj.Id == projectId).FirstOrDefaultAsync();

            ApiResponse<ProjectSchema> response = new();
            response.Data = new ProjectSchema();

            if (prj == null)
            {
                response.StatusCode = 404;
                response.Message = "Project Not Found";

                return response;
            }

            response.StatusCode = 200;
            response.Message = "Project Found";
            response.Data = prj;

            return response;
        }


        // Srvice to create a new project
        public async Task<ApiResponse<ProjectCreationDetailsDTO>> CreateNewProjectAsync(ProjectSchema projectSchema)
        {
            ApiResponse<ProjectCreationDetailsDTO> response = new();
            response.Data = new ProjectCreationDetailsDTO();

            ProjectSchema prj = await this.projectSchema.Find(prj => prj.ProjectName == projectSchema.ProjectName).FirstOrDefaultAsync();

            response.Data.ProjectName = projectSchema.ProjectName;
            response.Data.AdminEmail = projectSchema.AdminEmail;

            if (prj != null) 
            {
                response.StatusCode = 403;
                response.Message = "Project Already Exist";

                return response;
            }

            response.StatusCode = 201;
            response.Message = "Project Created";

            await this.projectSchema.InsertOneAsync(projectSchema);

            return response;
        }
    }
}
