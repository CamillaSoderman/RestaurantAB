using Microsoft.EntityFrameworkCore;
using RestaurantAB.Data;
using RestaurantAB.Models;
using RestaurantAB.Repository.IRepository;

namespace RestaurantAB.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly RestaurantABDbContext _context;
        public MenuRepository(RestaurantABDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddMenuItemAsync(Menu menu)
        {
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();

            return menu.Id;
        }

        public async Task<bool> DeleteMenuItemAsync(int menuId)
        {
            var rowsAffected = await _context.Menus.Where(m => m.Id == menuId).ExecuteDeleteAsync();

            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Menu>> GetAllMenuItemsAsync()
        {
            var menus = await _context.Menus.ToListAsync();

            return menus;
        }

        public async Task<Menu> GetMenuItemByIdAsync(int menuId)
        {
          var menuItem = await _context.Menus.FirstOrDefaultAsync(m => m.Id == menuId);

          // Logic for if menu item does not exist

          return menuItem;
        }

        public async Task<bool> UpdateMenuItemAsync(Menu menu)
        {
            _context.Menus.Update(menu);
            var result = await _context.SaveChangesAsync();

            if (result != 0)
            {
                return true;
            }

            return false;
        }
    }
}
