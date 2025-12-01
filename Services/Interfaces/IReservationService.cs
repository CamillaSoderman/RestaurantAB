using RestaurantAB.DTOs;
using RestaurantAB.DTOs.ReservationDTOs;

namespace RestaurantAB.Services.IServices
{
    public interface IReservationService
    {
        Task<List<ReservationAdminDTO>> GetAllReservationsAsync();
        Task<ReservationAdminDTO?> GetReservationByIdAsync(int id);
        Task<int> CreateReservationAsync(ReservationRequestDTO reservationRequestDTO);
        Task<bool> UpdateReservationAsync(int id, ReservationDTO reservationDTO);
        Task<bool> DeleteReservationAsync(int id);
        //Task<List<TableDTO>> GetAllAvailableTablesAsync(DateTime startTime, int numberOfGuests);
        Task<TableDTO?> GetBestAvailableTableAsync(DateTime startTime, int numberOfGuests);

        Task<TableDTO?> GetTableByIdAsync(int tableId);

        Task<int> CreateCustomerAsync(CustomerDTO customerDTO);
        Task<List<ReservationDTO>> GetAllReservationsForCustomerAsync(string email);

    }
}
