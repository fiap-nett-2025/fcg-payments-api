namespace FCG.Payments.Infra.Persistence.Config
{
    public class MongoDbOptions
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}
