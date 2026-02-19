using RestaurantAB.DTOs;
using RestaurantAB.DTOs.ReservationDTOs;
using RestaurantAB.DTOs.TableDTOs;

namespace RestaurantAB.Services.IServices
{
    public interface IReservationService
    {
        Task<List<ReservationAdminDTO>> GetAllReservationsAsync();
        Task<ReservationAdminDTO?> GetReservationByIdAsync(int id);
        Task<int> CreateReservationAsync(ReservationRequestDTO reservationRequestDTO);
        //Task<bool> UpdateReservationAsync(int id, ReservationDTO dto);
        Task<bool> DeleteReservationAsync(int id);

        Task<TableDTO?> GetBestAvailableTableAsync(DateTime startTime, int numberOfGuests);

        Task<TableDTO?> GetTableByIdAsync(int tableId);

        Task<int> CreateCustomerAsync(CustomerDTO customerDTO);
        Task<List<ReservationDTO>> GetAllReservationsForCustomerAsync(string email);
        Task<bool> UpdateReservationAsync(int id, ReservationUpdateDTO dto);
    }
}
