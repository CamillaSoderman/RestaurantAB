using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAB.DTOs;
using RestaurantAB.Models;
using RestaurantAB.Services.IServices;

namespace RestaurantAB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly RestaurantABDbContext _context;


        public MenuController(IMenuService menuService, RestaurantABDbContext context)
        {
            _menuService = menuService;
            _context = context;
        }

        // -------------------- ALL MENUS --------------------
        // GET: api/Menu/all
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Menu>>> GetAllMenuItems()
        {
            return await _context.Menus.ToListAsync();
        }

        // GET: api/Menu/admin
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllMenus()
        {
            var menus = await _context.Menus.ToListAsync();
            return Ok(menus);
        }

        // -------------------- POPULAR MENUS --------------------
        // GET: api/Menu/popular
        [HttpGet("popular")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Menu>>> GetPopularMenus()
        {
            return await _context.Menus
                .Where(m => m.IsPopular)
                .ToListAsync();
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<MenuDTO>> GetMenuItemById(int id)
        {
            var menuItem = await _menuService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound(new { message = "Menu item not found." });
            }
            return Ok(menuItem);
        }


        [HttpPost]
        [Route("createMenuItem")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateMenuItem([FromBody] MenuDTO menuDTO)
        {
            var newMenuId = await _menuService.CreateMenuItemAsync(menuDTO);
            return CreatedAtAction(nameof(GetMenuItemById), new { id = newMenuId }, menuDTO);
        }



        [HttpPut("{id:int}")]
        //[Route("updateMenuItem/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateMenuItem(int id, MenuDTO menuDTO)
        {
            if (id != menuDTO.MenuId)
            {
                return BadRequest(new { message = "Menu ID mismatch." });
            }
            var updated = await _menuService.UpdateMenuItemAsync(menuDTO);
            if (!updated)
            {
                return NotFound(new { message = "Menu item not found." });
            }
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        //[Route("deleteMenuItem/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteMenuItem(int id)
        {
            var deleted = await _menuService.DeleteMenuItemAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Menu item not found." });
            }
            return NoContent();
        }




    }
}
