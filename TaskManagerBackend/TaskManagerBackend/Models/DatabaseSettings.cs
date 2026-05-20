namespace TaskManagerBackend.Models
{
    public class DatabaseSettings
    {
        public string DatabaseConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UserCollectionName { get; set; }
        public string ProjectCollectionName { get; set; }
    }
}
