using E_Tech.Models;
using E_Tech.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Tech.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly decimal _shippingFee;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(IConfiguration configuration, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {

            _shippingFee = configuration.GetValue<decimal>("CartSettings:ShippingFee");
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            List<OrderItem> cartItems = CartHelper.GetCartItems(Request, Response, _context);
            decimal total = CartHelper.GetSubtotal(cartItems) + _shippingFee;

            string deliveryAddress = TempData["DeliveryAddress"] as string ?? "";
            TempData.Keep();

            ViewBag.DeliveryAddress = deliveryAddress;
            ViewBag.Total = total;
            return View();
        }
    }
}
