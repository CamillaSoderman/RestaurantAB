using RestaurantAB.DTOs.TableDTOs;

namespace RestaurantAB.Services.Interfaces
{
    public interface ITableService
    {
        Task<List<TableDTO>> GetAllTablesAsync();
        Task<TableDTO?> GetTableByIdAsync(int id);
        Task<int> CreateTableAsync(CreateTableDTO dto);
        Task<bool> UpdateTableAsync(int id, UpdateTableDTO dto);
        Task<bool> DeleteTableAsync(int id);
    }

}
