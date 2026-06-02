using CoffeeShop.Data;
using CoffeeShop.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Models.Services
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private CoffeeshopDbContext dbContext;

        public string? ShoppingCartId { get; set; }
        public List<ShoppingCartItem>? ShoppingCartItems { get; set; }

        public ShoppingCartRepository(CoffeeshopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public static ShoppingCartRepository GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            CoffeeshopDbContext context = services.GetService<CoffeeshopDbContext>() ??
                throw new Exception("Error initializing coffeeshopdbcontext");

            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();
            session?.SetString("CartId", cartId);

            return new ShoppingCartRepository(context) { ShoppingCartId = cartId };
        }

        public int RemoveFromCart(Product product)
        {
            var shoppingCartItem = dbContext.ShoppingCartItems
                .Include(s => s.Product)
                .FirstOrDefault(s => s.Product.Id == product.Id
                                  && s.ShoppingCartId == ShoppingCartId);

            var quantity = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Qty > 1)
                {
                    shoppingCartItem.Qty--;
                    quantity = shoppingCartItem.Qty;
                }
                else
                {
                    dbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            dbContext.SaveChanges();
            return quantity;
        }

        public void AddToCart(Product product)
        {
            var shoppingCartItem = dbContext.ShoppingCartItems
                .Include(s => s.Product)
                .FirstOrDefault(s => s.Product.Id == product.Id
                                  && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    Qty = 1
                };
                dbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Qty++;
            }

            dbContext.SaveChanges();
        }

        public List<ShoppingCartItem> GetAllShoppingCartItems()
        {
            if (string.IsNullOrEmpty(ShoppingCartId))
                return new List<ShoppingCartItem>();

            return ShoppingCartItems ??= dbContext.ShoppingCartItems
                .Where(s => s.ShoppingCartId == ShoppingCartId)
                .Include(p => p.Product)
                .ToList();
        }

        public decimal GetShoppingCartTotal()
        {
            if (string.IsNullOrEmpty(ShoppingCartId))
                return 0;

            var totalCost = dbContext.ShoppingCartItems
                .Where(s => s.ShoppingCartId == ShoppingCartId)
                .Include(s => s.Product)
                .Select(s => s.Product.Price * s.Qty)
                .Sum();

            return totalCost;
        }

        public void ClearCart()
        {
            var cartItems = dbContext.ShoppingCartItems
                .Where(s => s.ShoppingCartId == ShoppingCartId);
            dbContext.ShoppingCartItems.RemoveRange(cartItems);
            dbContext.SaveChanges();
        }
    }
}