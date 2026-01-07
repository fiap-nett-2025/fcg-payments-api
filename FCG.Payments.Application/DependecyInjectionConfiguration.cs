using FCG.Payments.Application.DTO.Game;
using FCG.Payments.Application.Handlers;
using FCG.Payments.Application.Publishers;
using FCG.Payments.Application.Publishers.Interfaces;
using FCG.Payments.Application.Services;
using FCG.Payments.Application.Services.Interfaces;
using FCG.Payments.Domain.Messaging.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FCG.Payments.Application
{
    public static class DependecyInjectionConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // Publishers
            services.AddTransient<IGameServicePublisher, GameServicePublisher>();
            services.AddTransient<IUserServicePublisher, UserServicePublisher>();

            // Services
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPaymentGatewayService, PaymentGatewayService>();
            services.AddTransient<IPricingService, PricingService>();

            // Handlers
            services.AddScoped<IMessageHandler<GameDto>, GameTestMessageHandler>();
            return services;
        }

        public static IServiceCollection ConfigureHttpClients(this IServiceCollection services, IConfigurationSection apiSection)
        {
            services.AddHttpClient("GamesApi", client =>
            {
                client.BaseAddress = new Uri(apiSection["GamesApiBaseUrl"] ?? "");
            });

            services.AddHttpClient("UsersApi", client =>
            {
                client.BaseAddress = new Uri(apiSection["UsersApiBaseUrl"] ?? "");
            });

            services.AddHttpClient("PaymentGateway", client =>
            {
                client.BaseAddress = new Uri(apiSection["PaymentGatewayBaseUrl"] ?? "");
            });

            return services;
        }
    }
}
