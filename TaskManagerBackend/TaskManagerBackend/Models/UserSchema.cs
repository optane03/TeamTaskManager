using MongoDB.Bson.Serialization.Attributes;

namespace TaskManagerBackend.Models
{
    public class UserSchema
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string Role { get; set; }
    }
}
