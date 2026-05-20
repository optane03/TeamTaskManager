using MongoDB.Bson.Serialization.Attributes;

namespace TaskManagerBackend.Models
{
    public class ProjectSchema
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id {  get; set; }
        public string ProjectName { get; set; }
        public string AdminEmail { get; set; }
        public string[] ProjectMembers { get; set; }
        public string ProjectStatus { get; set; } = "In Progress";
    }
}
