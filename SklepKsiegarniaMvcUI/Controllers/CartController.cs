using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace SklepKsiegarniaMvcUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;
        private readonly IUserOrderRepository _userOrderRepo;

        public CartController(ICartRepository cartRepo, IUserOrderRepository userOrderRepo)
        {
            _cartRepo = cartRepo;
            _userOrderRepo = userOrderRepo;
        }
        public async Task<IActionResult> AddItem(int bookId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartRepo.AddItem(bookId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int bookId)
        {
            var cartCount = await _cartRepo.RemoveItem(bookId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepo.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepo.GetCartItemCount();
            return Ok(cartItem);
        }

        public async Task<IActionResult> Checkout()
        {
            bool isCheckedOut = await _cartRepo.DoCheckout();
            if (!isCheckedOut)
                throw new Exception("Error in server side");
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RepeatOrder(int orderId)
        {
            try
            {
                var order = await _userOrderRepo.GetOrderById(orderId);

                if (order == null)
                    return NotFound(); 

                foreach (var orderDetail in order.OrderDetail)
                {
                    await _cartRepo.AddItem(orderDetail.BookId, orderDetail.Quantity);
                }


                return RedirectToAction("GetUserCart", "Cart");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home"); 
            }
        }

    }
}
