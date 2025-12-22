using FCG.Payments.Infra.Messaging.Config;
using FCG.Payments.Infra.Persistence;
using FCG.Payments.Infra.Persistence.Config;
using FCG.Payments.Infra.Persistence.Repository.Interfaces;
using FCG.Payments.Infra.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;
using FCG.Payments.Domain.Messaging.Interfaces;
using FCG.Payments.Infra.Messaging.Rabbit;
using Amazon.SQS;
using Microsoft.Extensions.Configuration;

namespace FCG.Payments.Infra
{
    public static class DependecyInjectionConfiguration
    {
        public static IServiceCollection ConfigureMongoDb(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
                var client = new MongoClient(settings.ConnectionString);
                return client.GetDatabase(settings.DatabaseName);
            });
            return services;
        }

        public static IServiceCollection ConfigureRabbitMq(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                var factory = new ConnectionFactory
                {
                    HostName = settings.HostName,
                    Port = settings.Port,
                    VirtualHost = settings.VirtualHost,
                    UserName = settings.UserName,
                    Password = settings.Password,
                    ClientProvidedName = settings.ClientProvidedName,
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
                };

                if (settings.UseSsl)
                {
                    factory.Ssl = new SslOption
                    {
                        Enabled = true,
                        ServerName = settings.HostName
                    };
                }

                return factory;
            });

            services.AddTransient<IQueuePublisher, RabbitMqPublisher>();
            services.AddTransient<IQueueConsumer, RabbitMqConsumer>();
            return services;
        }

        public static IServiceCollection ConfigureAmazonSQS(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonSQS>();
            return services;
        }

        public static IServiceCollection ConfigurePersistence(this IServiceCollection services)
        {
            services.AddTransient<IEventStore, MongoEventStore>();
            services.AddTransient<ICartRepository, CartRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}
