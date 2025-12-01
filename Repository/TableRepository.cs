using Microsoft.EntityFrameworkCore;
using RestaurantAB.Models;
using RestaurantAB.Repository.IRepository;

namespace RestaurantAB.Repository
{
    public class TableRepository : ITableRepository
    {
        private readonly RestaurantABDbContext _context;

        public TableRepository(RestaurantABDbContext context)
        {
            _context = context;
        }

        public async Task<List<Table>> GetAllAsync()
            => await _context.Tables.ToListAsync();

        public async Task<Table?> GetByIdAsync(int id)
            => await _context.Tables.FirstOrDefaultAsync(t => t.TableId == id);

        public async Task AddAsync(Table table)
        {
            _context.Tables.Add(table);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Table table)
        {
            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Table table)
        {
            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
        }
    }
}
