using RestaurantAB.Models;

using RestaurantAB.DTOs.ReservationDTOs;

namespace RestaurantAB.Repository.IRepository
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllReservationsAsync();
        Task<Reservation?> GetReservationByIdAsync(int resId);
        Task<int> CreateReservationAsync(Reservation reservation);
        Task<bool> UpdateReservationAsync(Reservation reservation);
        Task<bool> DeleteReservationAsync(int resId);
        Task<int> CreateCustomerAsync(Customer customer);
        Task<List<Reservation>> GetAllAvailableTables(DateTime startTime, int numberOfGuests);
        Task<List<Reservation>> GetReservationsForTableAsync(int tableId);

        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task<List<Reservation>> GetReservationsByCustomerEmailAsync(string email);

        Task<bool> IsTableOccupiedAsync(int tableId, DateTime startTime);

        Task<Table?> GetTableByIdAsync(int tableId);



    }
}
