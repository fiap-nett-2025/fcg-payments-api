using FCG.Payments.Application.DTO.Game;
using FCG.Payments.Application.Publishers.Interfaces;
using FCG.Payments.Application.Services.Interfaces;
using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Application.Services
{
    public class UserService(IUserServicePublisher userServicePublisher) : IUserService
    {
        public Task AddGamesInLibraryAsync(User user, params string[] gamesId)
        {
            var dto = new AddGameInLibraryDTO
            {
                UserId = user.Id,
                GamesId = gamesId
            };
            return userServicePublisher.AddGamesInLibraryAsync(dto);
        }
    }
}
