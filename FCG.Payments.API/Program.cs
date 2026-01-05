using FCG.Payments.API;
using FCG.Payments.API.Configurations;
using FCG.Payments.Application;
using FCG.Payments.Application.Middleware;
using FCG.Payments.Infra;
using FCG.Payments.Infra.Messaging.Config;
using FCG.Payments.Infra.Persistence.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

#region Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerConfiguration();
#endregion

#region JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var chaveSecreta = jwtSettings["Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta!)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("Authenticated", policy =>
        policy.RequireAuthenticatedUser());
});
#endregion

#region Dependency Injection

#region MongoDB
var mongoSection = builder.Configuration.GetSection("MongoDbSettings");

if (!mongoSection.Exists())
    throw new InvalidOperationException("Section 'MongoDbSettings' not found in configuration.");

builder.Services.Configure<MongoDbOptions>(mongoSection);
builder.Services.ConfigureMongoDb();
#endregion

var messagingSection = builder.Configuration.GetSection("Messaging");
if (!messagingSection.Exists())
    throw new InvalidOperationException("Section 'Messaging' not found in configuration.");

var queuesSection = messagingSection.GetSection("Queues");
builder.Services.Configure<QueuesOptions>(queuesSection);

builder.Services.ConfigureAmazonSQS(builder.Configuration);

#region RabbitMq (Not used)
//var rabbitSection = builder.Configuration.GetSection("RabbitMq");

//var rabbitSettingsSection = rabbitSection.GetSection("Settings");
//if (!rabbitSettingsSection.Exists())
//    throw new InvalidOperationException("Section 'RabbitMqSettings' not found in configuration.");
//builder.Services.Configure<RabbitMqOptions>(rabbitSettingsSection);

//var exchangesSection = rabbitSection.GetSection("Exchanges");
//if (!exchangesSection.Exists())
//    throw new InvalidOperationException("Section 'Exchanges' not found in configuration.");
//builder.Services.Configure<ExchangesOptions>(exchangesSection);

//builder.Services.ConfigureRabbitMq();
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

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

#region Pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwaggerConfiguration();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
#endregion

app.UseDeveloperExceptionPage();
app.UseExceptionHandler("/Error");
app.UseHsts();

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();