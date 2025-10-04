using FCG.Payments.Domain.Entities;
using FCG.Payments.Domain.Enums;

namespace FCG.Payments.Domain.Tests.Entities
{
    public class OrderTests
    {
        #region Constructor
        [Fact]
        public void Constructor_ShouldInitializeOrderCorrectly()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cartItems = new List<CartItem>
            {
                new(Guid.NewGuid().ToString(), 50), // Subtotal: 100
                new(Guid.NewGuid().ToString(), 30)  // Subtotal: 30
            };

            // Act
            var order = new Order(userId, cartItems);

            // Assert
            Assert.Equal(userId, order.UserId);
            Assert.Equal(2, order.Items.Count);
            Assert.Equal(80, order.Total);
            Assert.Equal(0, order.Discount);
            Assert.Equal(OrderStatus.Pending, order.Status);
            Assert.False(order.IsPaid);
        }
        #endregion

        #region ApplyDiscount
        [Fact]
        public void ApplyDiscount_ShouldUpdateTotalCorrectly()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cartItems = new List<CartItem>
            {
                new(Guid.NewGuid().ToString(), 50),
                new(Guid.NewGuid().ToString(), 30) 
            };
            var order = new Order(userId, cartItems);

            // Act
            order.ApplyDiscount(20);

            // Assert
            Assert.Equal(20, order.Discount);
            Assert.Equal(60, order.Total);
        }

        [Fact]
        public void ApplyDiscount_ShouldNotAllowNegativeTotal()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cartItems = new List<CartItem>
            {
                new(Guid.NewGuid().ToString(), 50)
            };
            var order = new Order(userId, cartItems);

            // Act
            order.ApplyDiscount(100);

            // Assert
            Assert.Equal(100, order.Discount);
            Assert.Equal(0, order.Total);
        }
        #endregion

        #region MarkAsPaid
        [Fact]
        public void MarkAsPaid_ShouldUpdateStatusToPaid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cartItems = new List<CartItem>
            {
                new(Guid.NewGuid().ToString(), 50) // Subtotal: 50
            };
            var order = new Order(userId, cartItems);

            // Act
            order.MarkAsPaid();

            // Assert
            Assert.Equal(OrderStatus.Paid, order.Status);
            Assert.True(order.IsPaid);
        }
        #endregion

        #region Cancel
        [Fact]
        public void Cancel_ShouldUpdateStatusToCancelled()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cartItems = new List<CartItem>
            {
                new(Guid.NewGuid().ToString(), 50) // Subtotal: 50
            };
            var order = new Order(userId, cartItems);

            // Act
            order.Cancel();

            // Assert
            Assert.Equal(OrderStatus.Cancelled, order.Status);
            Assert.False(order.IsPaid);
        }
        #endregion
    }
}
