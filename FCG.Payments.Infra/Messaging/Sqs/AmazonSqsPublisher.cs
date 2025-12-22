using Amazon.SQS.Model;
using Amazon.SQS;
using FCG.Payments.Domain.Messaging.Interfaces;
using Newtonsoft.Json;

namespace FCG.Payments.Infra.Messaging.Sqs
{
    public class AmazonSqsPublisher(IAmazonSQS sqs) : IQueuePublisher
    {
        public async Task PublishAsync<T>(T message, string queueName, string? exchange = null, CancellationToken cancellationToken = default)
        {
            var queueUrlResponse = await sqs.GetQueueUrlAsync(queueName, cancellationToken);

            var request = new SendMessageRequest
            {
                QueueUrl = queueUrlResponse.QueueUrl,
                MessageBody = JsonConvert.SerializeObject(message)
            };

            await sqs.SendMessageAsync(request, cancellationToken);
        }
    }
}
