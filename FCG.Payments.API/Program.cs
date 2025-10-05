using FCG.Payments.API.Configurations;
using FCG.Payments.Application;
using FCG.Payments.Application.Middleware;
using FCG.Payments.Application.Services;
using FCG.Payments.Application.Services.Interfaces;
using FCG.Payments.Infra.Data;
using FCG.Payments.Infra.Data.Repository;
using FCG.Payments.Infra.Data.Repository.Interfaces;
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

#region MongoDB
var mongoSection = builder.Configuration.GetSection("MongoDbSettings");

if (!mongoSection.Exists())
    throw new InvalidOperationException("Section 'MongoDbSettings' not found in configuration.");

builder.Services.Configure<MongoDbOptions>(mongoSection);
builder.Services.ConfigureMongoDb();
#endregion

#region Dependency Injection
builder.Services.AddTransient<IEventStore, MongoEventStore>();
builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IGameService, GameService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IPaymentGatewayService, PaymentGatewayService>();
#endregion

#region API clients
var apiSection = builder.Configuration.GetSection("API");
if (!apiSection.Exists())
    throw new InvalidOperationException("Section 'API' not found in configuration.");

builder.Services.AddHttpClient("GamesApi", client =>
{
    client.BaseAddress = new Uri(apiSection["GamesApiBaseUrl"] ?? "");
});

builder.Services.AddHttpClient("UsersApi", client =>
{
    client.BaseAddress = new Uri(apiSection["UsersApiBaseUrl"] ?? "");
});

builder.Services.AddHttpClient("PaymentGateway", client =>
{
    client.BaseAddress = new Uri(apiSection["PaymentGatewayBaseUrl"] ?? "");
});
#endregion

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