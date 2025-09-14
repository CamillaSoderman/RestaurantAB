using RestaurantAB.Models;


namespace RestaurantAB.Repository.IRepository
{
    public interface IMenuRepository
    {
        Task<List<Menu>> GetAllMenuItemsAsync();
        Task<Menu> GetMenuItemByIdAsync(int menuId);
        Task<int> AddMenuItemAsync(Menu menu);
        Task<bool> UpdateMenuItemAsync(Menu menu);
        Task<bool> DeleteMenuItemAsync(int menuId);
      
    }
}
