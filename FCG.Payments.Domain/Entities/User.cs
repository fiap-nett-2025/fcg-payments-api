namespace FCG.Payments.Domain.Entities
{
    public class User(string id, string token)
    {
        public Guid Id { get; init; } = Guid.Parse(id);
        public string Token { get; init; } = token;
    }
}
