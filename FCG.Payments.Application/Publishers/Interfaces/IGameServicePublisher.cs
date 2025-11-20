using FCG.Payments.Application.DTO.Game;

namespace FCG.Payments.Application.Publishers.Interfaces
{
    public interface IGameServicePublisher
    {
        Task IncreasePopularityAsync(IncreaseGamePopularityDTO dto);
    }
}
