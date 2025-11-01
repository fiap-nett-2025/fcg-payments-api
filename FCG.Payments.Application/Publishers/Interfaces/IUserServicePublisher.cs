using FCG.Payments.Application.DTO.Game;
using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Application.Publishers.Interfaces
{
    public interface IUserServicePublisher
    {
        Task AddGamesInLibraryAsync(AddGameInLibraryDTO dto);
    }
}