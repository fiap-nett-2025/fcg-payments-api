using FCG.Payments.Application.Services;
using FCG.Payments.Application.Services.Interfaces;
using FCG.Payments.Data;
using FCG.Payments.Data.Repository;
using FCG.Payments.Data.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region MongoDB
var section = builder.Configuration.GetSection("MongoDbSettings");
if (!section.Exists())
    throw new InvalidOperationException("Section 'MongoDbSettings' not found in configuration.");

builder.Services.ConfigureMongoDb();

builder.Services.AddTransient<IEventStore, MongoEventStore>();
builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<ICartService, CartService>();

builder.Services.Configure<MongoDbOptions>(section);
builder.Services.ConfigureMongoDb();
#endregion

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
