using Microsoft.EntityFrameworkCore;
using RestaurantAB.Data;
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

        public async Task<int> CreateReservationAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return reservation.ResId;
        }

        public async Task<bool> DeleteReservationAsync(int resId)
        {
           var rowsAffected = await _context.Reservations.Where(r => r.ResId == resId)
                .ExecuteDeleteAsync();

            if (rowsAffected > 0)
            {
                return true;
            }

                return false;
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations
                 .Include(r => r.Customer)
                 .ToListAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int resId)
        {
            return await _context.Reservations
                 .Include(r => r.Customer)
                 .FirstOrDefaultAsync(r => r.ResId == resId);
        }

        public async Task<bool> UpdateReservationAsync(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            var result = await _context.SaveChangesAsync();

            if (result != 0)
            {
                return true;
            }
            return false;
        }
        public async Task<int> CreateCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer.CustomerId;
        }
        public async Task<List<Reservation>> GetAllAvailableTables(DateTime startTime, int numberOfGuests)
        {
            var tables = await _context.Tables
                .Where(t => t.Capacity >= numberOfGuests)
                .ToListAsync();

            var availableTables = new List<Reservation>();

            foreach (var table in tables)
            {
                bool occupied = await _context.Reservations
                    .AnyAsync(r => r.TableId == table.Id &&
                   startTime >= r.StartTime.AddHours(-2) &&
                   startTime <= r.StartTime.AddHours(2));
                if (!occupied)
                {
                    var reservation = new Reservation
                    {
                        TableId = table.Id,
                        StartTime = startTime
                    };
                    availableTables.Add(reservation);
                }
            }
            return availableTables;
        }
        public async Task<List<Reservation>> GetReservationsForTableAsync(int tableId)
        {
            return await _context.Reservations
                .Where(r => r.TableId == tableId)
                .ToListAsync();
        }
        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerEmail == email);
        }

        public async Task<List<Reservation>> GetReservationsByCustomerEmailAsync(string email)
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .Where(r => r.Customer.CustomerEmail == email)
                .ToListAsync();
        }

        public async Task<Reservation?> GatReservationByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.ResId == id);
        }
    }
}
