namespace FCG.Payments.Infra.Data
{
    public class MongoDbOptions
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}
