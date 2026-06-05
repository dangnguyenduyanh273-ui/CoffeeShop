namespace CoffeeShop.Models.Interfaces
{
    public interface IOrderRepository
    {
        void PlaceOrder(Order order);
        List<Order> GetOrdersByUser(string? userId);
    }
}
