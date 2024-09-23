using E_Tech.DTOs;
using E_Tech.Models;
using E_Tech.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Security;

namespace E_Tech.Controllers
{
	[Authorize(Roles = "admin")]
	[Route("/Administrator/[Controller]/[action]")]
    public class ProductsController : Controller
    {
        //applicationDbContext
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly int _pageSize = 5;

        //constructor
        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index(int pageIndex, string? search, string? column, string? orderBy)
        {
            //List
            IQueryable<Product> products = _context.Products;

            //Search function
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search) || p.Brand.Contains(search) || p.Category.Contains(search));
            }

            //Order or Sort function
            string[] validColumns = { "Id", "Name", "Brand", "Category", "Price", "CreatedAt" };
            string[] validOrderBy = { "desc", "asc"};


            //this will be the default column to order by
            if (!validColumns.Contains(column))
            {
                column = "Id";
            }

            if (!validOrderBy.Contains(orderBy))
            {
                orderBy = "desc";
            }

            //Check if the column is Name
            if (column == "Name")
            {
                if (orderBy == "asc")
                {
                    products = products.OrderBy(p => p.Name);
                }
                else
                {
                    products = products.OrderByDescending(p => p.Name);
                }
            }

            //Check if the column is Brand
            else if (column == "Brand")
            {
                if (orderBy == "asc")
                {
                    products = products.OrderBy(p => p.Brand);
                }
                else
                {
                    products = products.OrderByDescending(p => p.Brand);
                }
            }

            //Check if the column is Category
            else if (column == "Category")
            {
                if (orderBy == "asc")
                {
                    products = products.OrderBy(p => p.Category);
                }
                else
                {
                    products = products.OrderByDescending(p => p.Category);
                }
            }

            //Check if the column is Price
            else if (column == "Price")
            {
                if (orderBy == "asc")
                {
                    products = products.OrderBy(p => p.Price);
                }
                else
                {
                    products = products.OrderByDescending(p => p.Price);
                }
            }

            //Check if the column is CreatedAt
            else if (column == "CreatedAt")
            {
                if (orderBy == "asc")
                {
                    products = products.OrderBy(p => p.CreatedAt);
                }
                else
                {
                    products = products.OrderByDescending(p => p.CreatedAt);
                }
            }

            //Check if the column is Id
            else
            {
                if (orderBy == "asc")
                {
                    products = products.OrderBy(p => p.Id);
                }
                else
                {
                    products = products.OrderByDescending(p => p.Id);
                }
            }


            //Pagination function
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            //get the total number of products
            decimal totalProducts = products.Count();
            //get the total number of pages
            int totalPages = (int)Math.Ceiling(totalProducts / _pageSize);
            //get the current page
            products = products.Skip((pageIndex - 1) * _pageSize).Take(_pageSize);

            //send the products to the view
            var productsList = products.ToList();
            ViewData["TotalPages"] = totalPages;
            ViewData["PageIndex"] = pageIndex;

            //search
            ViewData["Search"] = search ?? "";

            //sort or order View Data
            ViewData["Column"] = column;
            ViewData["OrderBy"] = orderBy;

            return View(productsList);
        }

        //create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if (productDto.ImageFileName == null)
            {
                ModelState.AddModelError("ImageFileName", "The image is required");
            }
            if (!ModelState.IsValid)
            {
                return View(productDto);

            }


            // save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFileName!.FileName);

            string imageFullPath = _environment.WebRootPath + "/products/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.ImageFileName.CopyTo(stream);
            }

            // save the new product in the database
            Product product = new Product()
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                Price = productDto.Price,
                Description = productDto.Description,
                ImageFile = newFileName,
                CreatedAt = DateTime.Now,
            };


            _context.Products.Add(product);
            _context.SaveChanges();


            return RedirectToAction("Index","Products");
        }

        //edit
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            // create productDto from product
            ProductDto productDto = new ProductDto()
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description,
            };


            ViewData["Id"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFile;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");



            return View(productDto);
        }
        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }


            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFile;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

                return View(productDto);
            }


            // update the image file if we have a new image file
            string newFileName = product.ImageFile;
            if (productDto.ImageFileName != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productDto.ImageFileName.FileName);

                string imageFullPath = _environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDto.ImageFileName.CopyTo(stream);
                }

                // delete the old image
                string oldImageFullPath = _environment.WebRootPath + "/products/" + product.ImageFile;
                System.IO.File.Delete(oldImageFullPath);
            }

            // update the product in the database
            product.Name = productDto.Name;
            product.Brand = productDto.Brand;
            product.Category = productDto.Category;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.ImageFile = newFileName;

            _context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }

        //delete
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            string imageFullPath = _environment.WebRootPath + "/products/" + product.ImageFile;
            System.IO.File.Delete(imageFullPath);

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }
    }

}

