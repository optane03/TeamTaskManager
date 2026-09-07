using MongoDB.Bson.Serialization.Attributes;

namespace TaskManagerBackend.DTO
{
    public class ProjectUpdationDTO
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectStatus { get; set; } = "In Progress";
    }
}
