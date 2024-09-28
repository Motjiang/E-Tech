using Microsoft.AspNetCore.Mvc;

namespace E_Tech.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
