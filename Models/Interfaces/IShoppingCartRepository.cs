using CoffeeShop.Models;

namespace CoffeeShop.Models.Interfaces
{
    public interface IShoppingCartRepository
    {
        void AddToCart(Product product);

        int RemoveFromCart(Product product);

        List<ShoppingCartItem> GetAllShoppingCartItems();

        void ClearCart();

        decimal GetShoppingCartTotal();

        List<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}