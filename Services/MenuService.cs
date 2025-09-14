using RestaurantAB.DTOs;
using RestaurantAB.Models;
using RestaurantAB.Repository.IRepository;
using RestaurantAB.Services.IServices;

namespace RestaurantAB.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepo;

        public MenuService(IMenuRepository menuRepo)
        {
            _menuRepo = menuRepo;
        }
        public async Task<int> CreateMenuItemAsync(MenuDTO menuDTO)
        {
            var menuItem = new Menu
            {
               Name = menuDTO.Name,
               Description = menuDTO.Description,
               Price = menuDTO.Price,
               IsPopular = menuDTO.IsPopular,
               ImageUrl = menuDTO.ImageUrl
               };

            var newMenuId = await _menuRepo.AddMenuItemAsync(menuItem);

            return newMenuId;

        }

        public async Task<bool> DeleteMenuItemAsync(int menuItemId)
        {
            var removed = await _menuRepo.DeleteMenuItemAsync(menuItemId);

            if (!removed)
            {
                return false;
            }
            return true;
        }

        public async Task<List<MenuDTO>> GetAllMenuItemsAsync()
        {
            var menuItems = await _menuRepo.GetAllMenuItemsAsync();

            var menuDTOs = menuItems.Select(m => new MenuDTO
            {
                MenuId = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                IsPopular = m.IsPopular,
                ImageUrl = m.ImageUrl
            }).ToList();

            return menuDTOs;
        }

        public async Task<MenuDTO> GetMenuItemByIdAsync(int menuId)
        {
            var menuItem = await _menuRepo.GetMenuItemByIdAsync(menuId);

            if (menuItem == null)
                {
                return null;
            }

            var menuDTO = new MenuDTO
            {
                MenuId = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsPopular = menuItem.IsPopular,
                ImageUrl = menuItem.ImageUrl
            };
            return menuDTO;
        }

        public async Task<bool> UpdateMenuItemAsync(MenuDTO menuDTO)
        {
            var menuItenm = await _menuRepo.GetMenuItemByIdAsync(menuDTO.MenuId);

            // Check if the menu item exists
            if (menuItenm == null)
            {
                return false;
            }
            // Update the fields of the existing menu item with values from the DTO
            if (!string.IsNullOrEmpty(menuDTO.Name))
            {
                menuItenm.Name = menuDTO.Name;
            }
            
            await _menuRepo.UpdateMenuItemAsync(menuItenm);

            return true;

        }
    }
}
