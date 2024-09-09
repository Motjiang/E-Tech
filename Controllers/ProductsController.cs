using E_Tech.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Tech.Controllers
{
    public class ProductsController : Controller
    {
        //applicationDbContext
        private readonly ApplicationDbContext _context;
        //constructor
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        //create
        public IActionResult Create()
        {
            return View();
        }
    }
}
