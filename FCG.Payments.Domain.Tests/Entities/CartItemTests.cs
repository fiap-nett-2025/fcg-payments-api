using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Domain.Tests.Entities
{
    public class CartItemTests
    {
        #region IncreaseQuantity
        [Fact]
        public void IncreaseQuantity_ValidAmount_IncreasesQuantity()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var initialQuantity = 2;
            var increaseBy = 3;
            var unitPrice = 50m;
            var cartItem = new CartItem(gameId, initialQuantity, unitPrice);

            // Act
            cartItem.IncreaseQuantity(increaseBy);

            // Assert
            Assert.Equal(initialQuantity + increaseBy, cartItem.Quantity);
            Assert.Equal(unitPrice, cartItem.UnitPrice);
            Assert.Equal((initialQuantity + increaseBy) * unitPrice, cartItem.Subtotal);
        }

        [Fact]
        public void IncreaseQuantity_ZeroAmount_NoChangeInQuantity()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var initialQuantity = 2;
            var increaseBy = 0;
            var unitPrice = 50m;
            var cartItem = new CartItem(gameId, initialQuantity, unitPrice);

            // Act
            cartItem.IncreaseQuantity(increaseBy);

            // Assert
            Assert.Equal(initialQuantity, cartItem.Quantity);
            Assert.Equal(unitPrice, cartItem.UnitPrice);
            Assert.Equal(initialQuantity * unitPrice, cartItem.Subtotal);
        }
        #endregion
    }
}
