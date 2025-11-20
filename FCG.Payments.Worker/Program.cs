using FCG.Payments.Application;
using FCG.Payments.Infra.Messaging.Config;
using FCG.Payments.Infra;
using FCG.Payments.Worker;
using System.Net;
using FCG.Payments.Infra.Persistence.Config;

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

#region Dependency Injection

#region MongoDB
var mongoSection = builder.Configuration.GetSection("MongoDbSettings");

if (!mongoSection.Exists())
    throw new InvalidOperationException("Section 'MongoDbSettings' not found in configuration.");

builder.Services.Configure<MongoDbOptions>(mongoSection);
builder.Services.ConfigureMongoDb();
#endregion

#region RabbitMq
var rabbitSection = builder.Configuration.GetSection("RabbitMq");

var rabbitSettingsSection = rabbitSection.GetSection("Settings");
if (!rabbitSettingsSection.Exists())
    throw new InvalidOperationException("Section 'RabbitMqSettings' not found in configuration.");
builder.Services.Configure<RabbitMqOptions>(rabbitSettingsSection);

var queuesSection = rabbitSection.GetSection("Queues");
if (!queuesSection.Exists())
    throw new InvalidOperationException("Section 'Queues' not found in configuration.");
builder.Services.Configure<QueuesOptions>(queuesSection);

builder.Services.ConfigureRabbitMq();
#endregion

#region API clients
var apiSection = builder.Configuration.GetSection("API");
if (!apiSection.Exists())
    throw new InvalidOperationException("Section 'API' not found in configuration.");

builder.Services.ConfigureHttpClients(apiSection);
#endregion

builder.Services.ConfigurePersistence()
                .ConfigureServices();
#endregion

var host = builder.Build();
host.Run();
