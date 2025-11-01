namespace FCG.Payments.Infra.Messaging.Config
{
    public class QueuesOptions
    {
        public string GamePopularityIncreasedQueue { get; set; } = string.Empty;
        public string UserGameLibraryAddedQueue { get; set; } = string.Empty;
    }
}
