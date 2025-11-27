using RestaurantAB.DTOs;

namespace RestaurantAB.Services.IServices
{
    public interface IMenuService
    {
        Task<List<MenuDTO>> GetAllMenuItemsAsync();
        Task<MenuDTO> GetMenuItemByIdAsync(int menuId);
        Task<int> CreateMenuItemAsync(MenuDTO menuDTO);
        Task<bool> DeleteMenuItemAsync(int menuId);
        Task<bool> UpdateMenuItemAsync(MenuDTO menuDTO);
    }
}
