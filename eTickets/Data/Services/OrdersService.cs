using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Services;

public class OrdersService : IOrdersService
{
    private readonly AppDbContext _context;

    public OrdersService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetOrdersByUserIDAsync(string userId)
    {
        var orders = await _context.Orders
            .Include(n => n.OrderItems)
            .ThenInclude(n => n.Movie)
            .Where(n => n.UserId == userId).ToListAsync();

        return orders;
    }

    public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string email)
    {
        var order = new Order
        {
            UserId = userId,
            Email = email
        };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        items.ForEach(async item =>
        {
            var orderItem = new OrderItem()
            { 
                Quantity = item.Qauntity,
                MovieId = item.Movie.Id,
                OrderId = order.Id,
                Price = item.Movie.Price,
            };
            await _context.OrderItems.AddAsync(orderItem);
        });
        await _context.SaveChangesAsync();
    }
}
