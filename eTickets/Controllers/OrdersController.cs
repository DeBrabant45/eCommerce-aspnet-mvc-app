using eTickets.Data.Cart;
using eTickets.Data.Services;
using eTickets.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eTickets.Controllers;
public class OrdersController : Controller
{
    private readonly IMovieService _movieService;
    private readonly IOrdersService _ordersService;
    private readonly ShoppingCart _shoppingCart;

    public OrdersController(IMovieService service, ShoppingCart shoppingCart, IOrdersService ordersService)
    {
        _movieService = service;
        _shoppingCart = shoppingCart;
        _ordersService = ordersService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = "";
        var orders = await _ordersService.GetOrdersByUserIDAsync(userId);
        return View(orders);
    }

    public IActionResult ShoppingCart()
    {
        var items = _shoppingCart.GetShoppingCartItems();
        _shoppingCart.ShoppingCartItems = items;

        var response = new ShoppingCartViewModel
        {
            ShoppingCart = _shoppingCart,
            ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal(),
        };

        return View(response);
    }

    public async Task<IActionResult> AddItemToShoppingCart(int id)
    {
        var item = await _movieService.GetMovieByIdAsync(id);
        if (item != null)
        {
            await _shoppingCart.AddItemToCart(item);
        }

        return RedirectToAction(nameof(ShoppingCart));
    }

    public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
    {
        var item = await _movieService.GetMovieByIdAsync(id);
        if (item != null)
        {
            await _shoppingCart.RemoveItemFromCart(item);
        }

        return RedirectToAction(nameof(ShoppingCart));
    }

    public async Task<IActionResult> CompleteOrder()
    {
        var items = _shoppingCart.GetShoppingCartItems();
        string userId = "";
        string userEmail = "";
        await _ordersService.StoreOrderAsync(items, userId, userEmail);
        await _shoppingCart.ClearShoppingCartAsync();
        return View("OrderCompleted");
    }
}
