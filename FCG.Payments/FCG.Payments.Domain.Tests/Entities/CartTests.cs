using FCG.Payments.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var gameId = Guid.NewGuid();
            var quantity = 2;
            var price = 50m;
            // Act
            cart.AddItem(gameId, quantity, price);

            // Assert
            Assert.Single(cart.Items);
            var item = cart.Items.First();

            Assert.Equal(gameId, item.GameId);
            Assert.Equal(quantity, item.Quantity);
            Assert.Equal(price, item.UnitPrice);
            Assert.Equal(quantity * price, item.Subtotal);
        }

        [Fact]
        public void AddItem_ExistingItem_IncreasesQuantity()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());
            var gameId = Guid.NewGuid();
            var initialQuantity = 2;
            var additionalQuantity = 3;
            var price = 50m;
            cart.AddItem(gameId, initialQuantity, price);

            // Act
            cart.AddItem(gameId, additionalQuantity, price);

            // Assert
            Assert.Single(cart.Items);
            var item = cart.Items.First();

            Assert.Equal(gameId, item.GameId);
            Assert.Equal(initialQuantity + additionalQuantity, item.Quantity);
            Assert.Equal(price, item.UnitPrice);
            Assert.Equal((initialQuantity + additionalQuantity) * price, item.Subtotal);
        }

        [Fact]
        public void AddItem_MultipleItems_AddsAllToCart()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());
            var gameId1 = Guid.NewGuid();
            var gameId2 = Guid.NewGuid();
            var quantity1 = 2;
            var quantity2 = 1;
            var price1 = 50m;
            var price2 = 30m;

            // Act
            cart.AddItem(gameId1, quantity1, price1);
            cart.AddItem(gameId2, quantity2, price2);

            // Assert
            Assert.Equal(2, cart.Items.Count);
            var item1 = cart.Items.First(i => i.GameId == gameId1);
            Assert.Equal(quantity1, item1.Quantity);
            Assert.Equal(price1, item1.UnitPrice);
            Assert.Equal(quantity1 * price1, item1.Subtotal);

            var item2 = cart.Items.First(i => i.GameId == gameId2);
            Assert.Equal(quantity2, item2.Quantity);
            Assert.Equal(price2, item2.UnitPrice);
            Assert.Equal(quantity2 * price2, item2.Subtotal);
        }
        #endregion

        #region RemoveItem
        [Fact]
        public void RemoveItem_ExistingItem_RemovesFromCart()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());
            var gameId = Guid.NewGuid();
            cart.AddItem(gameId, 2, 50m);

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
            var gameId = Guid.NewGuid();
            cart.AddItem(gameId, 2, 50m);
            var nonExistingGameId = Guid.NewGuid();

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
            cart.AddItem(Guid.NewGuid(), 2, 50m);
            cart.AddItem(Guid.NewGuid(), 1, 30m);

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
            cart.AddItem(Guid.NewGuid(), 2, 50m); // 100
            cart.AddItem(Guid.NewGuid(), 1, 30m); // 30
            cart.AddItem(Guid.NewGuid(), 3, 20m); // 60

            // Act
            var total = cart.GetTotal();

            // Assert
            Assert.Equal(190m, total);
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
