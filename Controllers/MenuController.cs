using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantAB.Data;
using RestaurantAB.DTOs;

namespace RestaurantAB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly RestaurantABDbContext _context;

        public MenuController(RestaurantABDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous] // Allow anonymous access to this endpoint
        public IActionResult GetMenuItems()
        {
            var menuItems = _context.Menus.ToList();
            return Ok(menuItems);
        }

        // Admin add menu item
        [HttpPost]
        [Authorize(Roles = "Admin")] // Only Admins can access this endpoint
        public IActionResult AddMenuItem(MenuDTO menuDTO)
        {
            var menuItem = new MenuDTO
            {
                Name = menuDTO.Name,
                Description = menuDTO.Description,
                Price = menuDTO.Price,
                IsPopular = menuDTO.IsPopular,
                ImageUrl = menuDTO.ImageUrl
            };

            var menuItems = _context.Menus.ToList();
            return Ok(menuItems);
        }

        // Admin update menu item
        [HttpPut]
        [Authorize(Roles = "Admin")] // Only Admins can access this endpoint
        public IActionResult UpdateMenuItem(int id, MenuDTO menuDTO)
        {
            var menuItem = _context.Menus.Find(id);
            if (menuItem == null)
            {
                return NotFound(new { message = "Menu item not found." });
            }
            menuItem.Name = menuDTO.Name;
            menuItem.Description = menuDTO.Description;
            menuItem.Price = menuDTO.Price;
            menuItem.IsPopular = menuDTO.IsPopular;
            menuItem.ImageUrl = menuDTO.ImageUrl;
            _context.SaveChanges();
            return Ok(menuItem);
        }

        // Admin delete menu item
        [HttpDelete]
        [Authorize(Roles = "Admin")] // Only Admins can access this endpoint
        public IActionResult DeleteMenuItem(int id)
        {
            var menuItem = _context.Menus.Find(id);
            if (menuItem == null)
            {
                return NotFound(new { message = "Menu item not found." });
            }
            _context.Menus.Remove(menuItem);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
