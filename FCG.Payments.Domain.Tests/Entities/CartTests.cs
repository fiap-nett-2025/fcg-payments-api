using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Domain.Tests.Entities
{
    public class CartTests
    {
        #region AddItem
        [Fact]
        public void AddItem_NewItem_AddsToCart()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());
            var gameId = Guid.NewGuid().ToString();
            var price = 50m;
            // Act
            cart.AddItem(gameId, price);

            // Assert
            Assert.Single(cart.Items);
            var item = cart.Items.First();

            Assert.Equal(gameId, item.GameId);
            Assert.Equal(price, item.UnitPrice);
        }

        [Fact]
        public void AddItem_ExistingItem_IncreasesQuantity()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());
            var gameId = Guid.NewGuid().ToString();
            var price = 50m;
            cart.AddItem(gameId, price);

            // Act
            cart.AddItem(gameId, price);

            // Assert
            Assert.Single(cart.Items);
            var item = cart.Items.First();

            Assert.Equal(gameId, item.GameId);
            Assert.Equal(price, item.UnitPrice);
        }

        [Fact]
        public void AddItem_MultipleItems_AddsAllToCart()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());
            var gameId1 = Guid.NewGuid().ToString();
            var gameId2 = Guid.NewGuid().ToString();
            var price1 = 50m;
            var price2 = 30m;

            // Act
            cart.AddItem(gameId1, price1);
            cart.AddItem(gameId2, price2);

            // Assert
            Assert.Equal(2, cart.Items.Count);
            var item1 = cart.Items.First(i => i.GameId == gameId1);
            Assert.Equal(price1, item1.UnitPrice);

            var item2 = cart.Items.First(i => i.GameId == gameId2);
            Assert.Equal(price2, item2.UnitPrice);
        }
        #endregion

        #region RemoveItem
        [Fact]
        public void RemoveItem_ExistingItem_RemovesFromCart()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());
            var gameId = Guid.NewGuid().ToString();
            cart.AddItem(gameId, 50m);

            // Act
            cart.RemoveItem(gameId);

            // Assert
            Assert.Empty(cart.Items);
        }

        [Fact]
        public void RemoveItem_NonExistingItem_DoesNothing()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());
            var gameId = Guid.NewGuid().ToString();
            cart.AddItem(gameId, 50m);
            var nonExistingGameId = Guid.NewGuid().ToString();

            // Act
            cart.RemoveItem(nonExistingGameId);

            // Assert
            Assert.Single(cart.Items);
        }
        #endregion

        #region Clear
        [Fact]
        public void Clear_CartWithItems_EmptiesCart()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());
            cart.AddItem(Guid.NewGuid().ToString(), 50m);
            cart.AddItem(Guid.NewGuid().ToString(), 30m);

            // Act
            cart.Clear();

            // Assert
            Assert.Empty(cart.Items);
        }
        #endregion

        #region GetTotal
        [Fact]
        public void GetTotal_CartWithItems_ReturnsCorrectTotal()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());
            cart.AddItem(Guid.NewGuid().ToString(), 50m); 
            cart.AddItem(Guid.NewGuid().ToString(), 30m); 
            cart.AddItem(Guid.NewGuid().ToString(), 20m); 

            // Act
            var total = cart.GetTotal();

            // Assert
            Assert.Equal(100m, total);
        }

        [Fact]
        public void GetTotal_EmptyCart_ReturnsZero()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());

            // Act
            var total = cart.GetTotal();

            // Assert
            Assert.Equal(0m, total);
        }
        #endregion
    }
}
