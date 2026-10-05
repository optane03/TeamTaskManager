using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using TaskManagerBackend.DTO;
using TaskManagerBackend.Errors;
using TaskManagerBackend.Models;
using TaskManagerBackend.Responses;

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
        public async Task<ApiResponseWithData<List<ProjectSchema>>> GetAllProjectDetailsAsync(string userEmail)
        {
            ApiResponseWithData<List<ProjectSchema>> response = new();
            response.Data = new List<ProjectSchema>();

            response.Data = await projectSchema.Find(prj => prj.UserEmail == userEmail).ToListAsync();
            response.StatusCode = 200;
            response.Message = "Found All Project";

            return response;
        }


        // Service to get a particullar project details
        public async Task<ApiResponseWithData<ProjectSchema>> GetProjectDetailsAsync(string projectId)
        {
            ProjectSchema prj = await projectSchema.Find(prj => prj.Id == projectId).FirstOrDefaultAsync();

            ApiResponseWithData<ProjectSchema> response = new();
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
        public async Task<ApiResponseWithData<ProjectCreationDetailsDTO>> CreateNewProjectAsync(ProjectSchema projectSchema)
        {
            ApiResponseWithData<ProjectCreationDetailsDTO> response = new();
            response.Data = new ProjectCreationDetailsDTO();

            ProjectSchema prj = await this.projectSchema.Find(prj => prj.ProjectName == projectSchema.ProjectName).FirstOrDefaultAsync();

            response.Data.ProjectName = projectSchema.ProjectName;

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


        // Service to update a project
        public async Task<ApiResponse> UpdateProject(ProjectUpdationDTO project)
        {
            ApiResponse response = new();

            ProjectSchema prj = await projectSchema.Find(prj => prj.Id == project.Id).FirstOrDefaultAsync();

            if (prj == null)
            {
                response.StatusCode = 404;
                response.Message = "Project Not Found";

                return response;
            }

            if (string.IsNullOrEmpty(project.Id)) 
            {
                response.StatusCode = 400;
                response.Message = "Project id is missing";

                return response;
            }

            var filter = Builders<ProjectSchema>.Filter.Eq(prj => prj.Id, project.Id);

            var updateBuilder = Builders<ProjectSchema>.Update;
            var updates = new List<UpdateDefinition<ProjectSchema>>();

            if (project.ProjectName != null)
                updates.Add(updateBuilder.Set(prj => prj.ProjectName, project.ProjectName));

            if (project.ProjectStatus != null)
                updates.Add(updateBuilder.Set(prj => prj.ProjectStatus, project.ProjectStatus));

            var update = updateBuilder.Combine(updates);

            await projectSchema.UpdateOneAsync(filter, update);

            response.StatusCode = 200;
            response.Message = "Project updated successfully";

            return response;
        }


        // Service to delete a project
        public async Task<ApiResponse> DeleteProject(string projectId)
        {
            ApiResponse response = new();

            ProjectSchema prj = await projectSchema.Find(prj => prj.Id == projectId).FirstOrDefaultAsync();

            if (prj == null)
            {
                response.StatusCode = 404;
                response.Message = "Project Not Found";

                return response;
            }

            if (string.IsNullOrEmpty(projectId))
            {
                response.StatusCode = 400;
                response.Message = "Project id is required";
            }

            await projectSchema.DeleteOneAsync(pr => pr.Id == projectId);

            response.StatusCode = 200;
            response.Message = "Project is deleted";

            return response;
        }
    }
}
