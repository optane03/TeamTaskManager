using MongoDB.Bson.Serialization.Attributes;

namespace TaskManagerBackend.DTO
{
    public class UserDetailsDTO
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Role { get; set; }
        public string? Token { get; set; }
    }    
    
}
