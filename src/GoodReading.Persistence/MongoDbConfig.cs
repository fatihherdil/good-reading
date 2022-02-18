namespace GoodReading.Persistence
{
    public class MongoDbConfig : IConnectionStringConfig, IDatabaseConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
