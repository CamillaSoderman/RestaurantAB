using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAB.DTOs.ReservationDTOs;
using RestaurantAB.DTOs.TableDTOs;
using RestaurantAB.Services.IServices;

namespace RestaurantAB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // -------------------- AVAILABLE TABLE --------------------
        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<ActionResult<TableDTO>> GetAvailableTable(
            [FromQuery] DateTime startTime,
            [FromQuery] int numberOfGuests)
        {
            if (numberOfGuests <= 0)
                return BadRequest("Invalid number of guests.");

            var table = await _reservationService.GetBestAvailableTableAsync(startTime, numberOfGuests);

            if (table == null)
                return NotFound("Inga lediga bord.");

            return Ok(table);
        }

        // -------------------- CREATE RESERVATION --------------------
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ReservationAdminDTO>> CreateReservation([FromBody] ReservationRequestDTO request)
        {
            if (request == null)
                return BadRequest(new { message = "Invalid reservation request." });

            var newResId = await _reservationService.CreateReservationAsync(request);
            var reservation = await _reservationService.GetReservationByIdAsync(newResId);

            return Ok(reservation);
        }

        // -------------------- ALL RESERVATIONS (ADMIN) --------------------
        [HttpGet("allReservations")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ReservationAdminDTO>>> GetAllReservations()
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            return Ok(reservations);
        }

        // -------------------- RESERVATION BY ID (ADMIN) --------------------
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReservationAdminDTO>> GetReservationById(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
                return NotFound(new { message = "Reservation not found." });

            return Ok(reservation);
        }

        // -------------------- CUSTOMER RESERVATIONS --------------------
        [HttpGet("customerReservations")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReservationByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "Email parameter is required." });

            var reservations = await _reservationService.GetAllReservationsForCustomerAsync(email);

            if (!reservations.Any())
                return NotFound(new { message = "No reservations found." });

            return Ok(reservations);
        }

        // -------------------- UPDATE RESERVATION (ADMIN) --------------------
        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateReservation(int id, [FromBody] ReservationUpdateDTO dto)
        {
            var updated = await _reservationService.UpdateReservationAsync(id, dto);

            if (!updated)
                return NotFound(new { message = "Reservation not found." });

            return NoContent();
        }

        // -------------------- DELETE RESERVATION --------------------
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var deleted = await _reservationService.DeleteReservationAsync(id);

            if (!deleted)
                return NotFound(new { message = "Reservation not found." });

            return NoContent();
        }
    }
}





