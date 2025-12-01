using Microsoft.EntityFrameworkCore;
using RestaurantAB.Models;
using RestaurantAB.Repository.IRepository;

namespace RestaurantAB.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly RestaurantABDbContext _context;

        public ReservationRepository(RestaurantABDbContext context)
        {
            _context = context;
        }

        // -------------------- CREATE --------------------
        public async Task<int> CreateReservationAsync(Reservation reservation)
        {
            // Always enforce 2-hour duration
            reservation.EndTime = reservation.StartTime.AddHours(2);

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return reservation.Id;
        }

        public async Task<int> CreateCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer.CustomerId;
        }

        // -------------------- DELETE --------------------
        public async Task<bool> DeleteReservationAsync(int resId)
        {
            var rowsAffected = await _context.Reservations
                .Where(r => r.Id == resId)
                .ExecuteDeleteAsync();

            return rowsAffected > 0;
        }

        // -------------------- READ --------------------
        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .ToListAsync();
        }

        public async Task<Reservation?> GetReservationByIdAsync(int resId)
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == resId);
        }

        public async Task<List<Reservation>> GetReservationsForTableAsync(int tableId)
        {
            return await _context.Reservations
                .Where(r => r.TableId == tableId)
                .Include(r => r.Customer)
                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerEmail == email);
        }

        public async Task<List<Reservation>> GetReservationsByCustomerEmailAsync(string email)
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .Where(r => r.Customer.CustomerEmail == email)
                .ToListAsync();
        }

        // -------------------- UPDATE --------------------
        public async Task<bool> UpdateReservationAsync(Reservation reservation)
        {
            // Always enforce 2-hour duration
            reservation.EndTime = reservation.StartTime.AddHours(2);

            _context.Reservations.Update(reservation);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        // -------------------- TABLES --------------------
        public async Task<Table?> GetTableByIdAsync(int tableId)
        {
            return await _context.Tables.FindAsync(tableId);
        }

        //public async Task<List<Table>> GetAllAvailableTablesAsync(DateTime startTime, int numberOfGuests)
        //{
        //    var tables = await _context.Tables
        //        .Where(t => t.Capacity >= numberOfGuests)
        //        .ToListAsync();

        //    var availableTables = new List<Table>();

        //    foreach (var table in tables)
        //    {
        //        bool occupied = await _context.Reservations
        //            .AnyAsync(r => r.TableId == table.TableId &&
        //                           r.StartTime < startTime.AddHours(2) &&
        //                           r.EndTime > startTime);

        //        if (!occupied)
        //        {
        //            availableTables.Add(table);
        //        }
        //    }

        //    return availableTables;
        //}

        public async Task<List<Table>> GetAllTablesAsync()
        {
            return await _context.Tables
                .Include(t => t.Reservations)   // IMPORTANT for availability checks
                .ToListAsync();
        }

        public async Task<bool> IsTableOccupiedAsync(int tableId, DateTime startTime)
        {
            var endTime = startTime.AddHours(2);

            return await _context.Reservations
                .AnyAsync(r => r.TableId == tableId &&
                               r.StartTime < endTime &&
                               r.EndTime > startTime);
        }
        public async Task<List<Table>> GetAllAvailableTablesAsync(DateTime startTime, int numberOfGuests)
        {
            // Hämta bord som kan rymma gäster
            var candidateTables = await _context.Tables
                .Where(t => t.Capacity >= numberOfGuests)
                .ToListAsync();

            var availableTables = new List<Table>();

            foreach (var table in candidateTables)
            {
                // Kontrollera om bordet är ledigt under valda tider
                bool occupied = await _context.Reservations
                    .AnyAsync(r => r.TableId == table.TableId &&
                                   r.StartTime < startTime.AddHours(2) &&
                                   r.EndTime > startTime);

                if (!occupied)
                    availableTables.Add(table);
            }

            return availableTables;
        }

        // ✅ Hitta bästa bord (minsta bord som rymmer gäster)
        public async Task<Table?> GetBestAvailableTableAsync(DateTime startTime, int numberOfGuests)
        {
            var tables = await GetAllAvailableTablesAsync(startTime, numberOfGuests);

            // Välj bord med minsta capacity som ändå rymmer gäster
            return tables.OrderBy(t => t.Capacity).FirstOrDefault();
        }





    }
}
