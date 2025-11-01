using FCG.Payments.Infra.Messaging.Config;
using FCG.Payments.Infra;
using FCG.Payments.Worker;

var builder = Host.CreateApplicationBuilder(args);

#region RabbitMq
var rabbitSection = builder.Configuration.GetSection("RabbitMqSettings");

if (!rabbitSection.Exists())
    throw new InvalidOperationException("Section 'RabbitMqSettings' not found in configuration.");

builder.Services.Configure<RabbitMqOptions>(rabbitSection);
builder.Services.ConfigureRabbitMq();
#endregion

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
