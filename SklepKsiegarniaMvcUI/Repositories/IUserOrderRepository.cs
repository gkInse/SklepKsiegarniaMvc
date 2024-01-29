namespace SklepKsiegarniaMvcUI.Repositories
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders();
        Task<Order> GetOrderById(int orderId);
    }
}