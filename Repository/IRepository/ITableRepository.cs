using RestaurantAB.Models;

namespace RestaurantAB.Repository.IRepository
{
    public interface ITableRepository
    {
        Task<List<Table>> GetAllAsync();
        Task<Table?> GetByIdAsync(int id);
        Task AddAsync(Table table);
        Task UpdateAsync(Table table);
        Task DeleteAsync(Table table);
    }

}
