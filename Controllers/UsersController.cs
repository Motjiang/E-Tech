using E_Tech.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Tech.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly int _pageSize = 5;

        // Constructor
        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index(int? pageIndex)
        {

            IQueryable<ApplicationUser> query =  _userManager.Users.OrderByDescending(u => u.CreatedAt);

            // Paginate the users
            if(pageIndex == null || pageIndex < 1)
            {
                pageIndex = 1;
            }

            // Calculate the total pages
            decimal pageCount = query.Count();
            int totalPages = (int)Math.Ceiling(pageCount / _pageSize);
            query = query.Skip(((int)pageIndex- 1) * _pageSize).Take(_pageSize);

            // Get the users
            var users = query.ToList();

            // Pass the data to the view
            ViewData["pageIndex"] = pageIndex;
            ViewData["totalPages"] = totalPages;

            // Return the view
            return View(users);
        }
    }
}
