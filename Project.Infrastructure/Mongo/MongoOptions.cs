namespace Project.Infrastructure.Mongo
{
    public class MongoOptions
    {
        public const string SectionName = "mongo";
        public string ConnectionString { get; set; }
        public string DataBase { get; set; }
        public bool Seed { get; set; }

    }
}
