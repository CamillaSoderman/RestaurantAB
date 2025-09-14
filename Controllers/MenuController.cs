using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantAB.Data;
using RestaurantAB.DTOs;
using RestaurantAB.Repository.IRepository;
using RestaurantAB.Services.IServices;

namespace RestaurantAB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        //private readonly RestaurantABDbContext _context;

        public MenuController(IMenuService menuService)
        {
           _menuService = menuService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<MenuDTO>>> GetAllMenuItems()
        {
                var menuItems = await _menuService.GetAllMenuItemsAsync();
                return Ok(menuItems);
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
        public async Task<ActionResult> CreateMenuItem(MenuDTO menuDTO)
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
