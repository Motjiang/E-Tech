using E_Tech.Models;
using E_Tech.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Tech.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly int pageSize = 8;

        public StoreController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index(int pageIndex, string? search, string? brand, string? category, string? sort)
        {
            // list of products
            IQueryable<Product> products = context.Products;

            // search functionality
            if (search != null && search.Length > 0)
            {
                products = products.Where(p => p.Name.Contains(search));
            }


            // filter functionality
            if (brand != null && brand.Length > 0)
            {
                products = products.Where(p => p.Brand.Contains(brand));
            }

            if (category != null && category.Length > 0)
            {
                products = products.Where(p => p.Category.Contains(category));
            }

            // sort functionality
            if (sort == "price_asc")
            {
                products = products.OrderBy(p => p.Price);
            }
            else if (sort == "price_desc")
            {
                products = products.OrderByDescending(p => p.Price);
            }
            else
            {
                // newest products first
                products = products.OrderByDescending(p => p.Id);
            }



            // pagination functionality
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            // count total pages
            decimal count = products.Count();
            int totalPages = (int)Math.Ceiling(count / pageSize);
            products = products.Skip((pageIndex - 1) * pageSize).Take(pageSize);


            var productList = products.ToList();
            // pass data to view
            ViewBag.Products = productList;
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPages = totalPages;

            // pass search data to view
            var storeSearchModel = new StoreSearchModel()
            {
                Search = search,
                Brand = brand,
                Category = category,
                Sort = sort
            };

            return View(storeSearchModel);

        }

        public IActionResult Details(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Store");
            }

            return View(product);
        }
    }
}
