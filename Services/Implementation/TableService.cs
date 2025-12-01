using RestaurantAB.DTOs.TableDTOs;
using RestaurantAB.Models;
using RestaurantAB.Repository.IRepository;
using RestaurantAB.Services.Interfaces;

namespace RestaurantAB.Services.Implementation
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _repo;

        public TableService(ITableRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<TableDTO>> GetAllTablesAsync()
        {
            var tables = await _repo.GetAllAsync();
            return tables.Select(t => new TableDTO
            {
                TableId = t.TableId,
                Capacity = t.Capacity
            }).ToList();
        }

        public async Task<TableDTO?> GetTableByIdAsync(int id)
        {
            var table = await _repo.GetByIdAsync(id);
            if (table == null) return null;

            return new TableDTO
            {
                TableId = table.TableId,
                Capacity = table.Capacity
            };
        }

        public async Task<int> CreateTableAsync(CreateTableDTO dto)
        {
            var table = new Table
            {
                Capacity = dto.Capacity
            };

            await _repo.AddAsync(table);
            return table.TableId;
        }

        public async Task<bool> UpdateTableAsync(int id, UpdateTableDTO dto)
        {
            var table = await _repo.GetByIdAsync(id);
            if (table == null) return false;

            table.Capacity = dto.Capacity;
            await _repo.UpdateAsync(table);
            return true;
        }

        public async Task<bool> DeleteTableAsync(int id)
        {
            var table = await _repo.GetByIdAsync(id);
            if (table == null) return false;

            await _repo.DeleteAsync(table);
            return true;
        }
    }

}
