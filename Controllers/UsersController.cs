using E_Tech.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Users(int? pageIndex)
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

        public async Task<IActionResult> Details(string? id)
        {
            // Check if the id is null
            if (id == null)
            {
                return RedirectToAction("Users", "Users");
            }

            // Get the user by id
            var appUser = await _userManager.FindByIdAsync(id);

            // Check if the user is null
            if (appUser == null)
            {
                return RedirectToAction("Users", "Users");
            }

            // Get the roles of the user
            ViewBag.Roles = await _userManager.GetRolesAsync(appUser);

            // get available roles
            var availableRoles = _roleManager.Roles.ToList();
            // Create a list of select list items
            var items = new List<SelectListItem>();
            // Loop through the available roles
            foreach (var role in availableRoles)
            {
                items.Add(
                    new SelectListItem
                    {
                        Text = role.NormalizedName,
                        Value = role.Name,
                        // Check if the user is in the role
                        Selected = await _userManager.IsInRoleAsync(appUser, role.Name!),
                    });
            }

            // Pass the roles to the view
            ViewBag.SelectItems = items;

            // Return the view
            return View(appUser);
        }

    }
}
