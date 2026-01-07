using Amazon.SQS;
using Amazon.SQS.Model;
using FCG.Payments.Domain.Messaging.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FCG.Payments.Infra.Messaging.Sqs
{
    public class AmazonSqsConsumer(
        IAmazonSQS sqs,
        ILogger<AmazonSqsConsumer> logger
    ) : IQueueConsumer
    {
        public async Task StartAsync<T>(
            string queueName,
            IMessageHandler<T> handler,
            CancellationToken cancellationToken = default)
        {
            if (handler is null)
                throw new ArgumentNullException(nameof(handler), 
                    $"IMessageHandler<{typeof(T).Name}> is null. Check DI registration.");

            // Resolve URL da fila
            var queueUrlResponse = await sqs.GetQueueUrlAsync(queueName, cancellationToken);
            var queueUrl = queueUrlResponse.QueueUrl;

            logger.LogInformation("Listening SQS queue {QueueName}, {@QueueResponse}", queueName, JsonConvert.SerializeObject(queueUrlResponse));

            while (!cancellationToken.IsCancellationRequested)
            {
                var receiveRequest = new ReceiveMessageRequest
                {
                    QueueUrl = queueUrl,
                    MaxNumberOfMessages = 10,
                    WaitTimeSeconds = 20, // long polling
                    VisibilityTimeout = 30
                };

                var response = await sqs.ReceiveMessageAsync(receiveRequest, cancellationToken);

                logger.LogInformation("Response from {QueueName}: {@Response}", queueName, JsonConvert.SerializeObject(response));

                foreach (var message in response?.Messages ?? [])
                {
                    try
                    {
                        var body = message.Body;
                        var obj = JsonConvert.DeserializeObject<T>(body)
                                  ?? throw new InvalidOperationException("Failed to deserialize message");

                        await handler.HandleAsync(obj, cancellationToken);

                        // ACK = delete message
                        await sqs.DeleteMessageAsync(queueUrl, message.ReceiptHandle, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(
                            ex,
                            "Error processing message from queue {QueueName}: {Body}",
                            queueName,
                            message.Body
                        );
                    }
                }
            }
        }
    }
}
