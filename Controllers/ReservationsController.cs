using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAB.DTOs;
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
        private readonly RestaurantABDbContext _context;

        public ReservationsController(IReservationService reservationService, RestaurantABDbContext context)
        {
            _reservationService = reservationService;
            _context = context;
        }
        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<ActionResult<TableDTO>> GetAvailableTable(
    [FromQuery] DateTime startTime,
    [FromQuery] int numberOfGuests)
        {
            if (numberOfGuests <= 0) return BadRequest("Invalid number of guests.");

            // Endast bokningar mellan 10-22
            if (startTime.Hour < 10 || startTime.Hour >= 22)
                return BadRequest("Bokningar är endast tillåtna mellan 10:00 och 22:00.");

            var table = await _reservationService.GetBestAvailableTableAsync(startTime, numberOfGuests);

            if (table == null) return NotFound("Inga lediga bord.");

            return Ok(table);
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ReservationDTO>> CreateReservation([FromBody] ReservationRequestDTO request)
        {
            if (request == null)
                return BadRequest(new { message = "Invalid reservation request." });

            try
            {
                var newResId = await _reservationService.CreateReservationAsync(request);

                // Fetch the reservation to return details
                var reservation = await _reservationService.GetReservationByIdAsync(newResId);

                return Ok(reservation);
            }
            catch (InvalidOperationException ex)
            {
                // e.g., table already booked
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating reservation: {ex}");
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

        // -------------------- AVAILABLE TABLES --------------------
        //[HttpGet("available")]
        //[AllowAnonymous]
        //public async Task<ActionResult<List<TableDTO>>> GetAvailableTables(DateTime startTime, int NumberOfGuests)
        //{
        //    var availableTables = await _reservationService.GetAllAvailableTablesAsync(startTime, NumberOfGuests);

        //    if (availableTables == null || !availableTables.Any())
        //        return NotFound(new { message = "No available tables found." });

        //    return Ok(availableTables);
        //}


        // -------------------- ALL RESERVATIONS (ADMIN) --------------------
        [HttpGet("allReservations")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ReservationAdminDTO>>> GetAllReservations()
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            if (reservations == null || !reservations.Any())
                return NotFound(new { message = "No reservations found." });

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

        // -------------------- CREATE RESERVATION --------------------
        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<ActionResult<ReservationDTO>> CreateReservation(ReservationRequestDTO request)
        //{
        //    try
        //    {
        //        var reservationId = await _reservationService.CreateReservationAsync(request);

        //        var reservation = await _context.Reservations.FindAsync(reservationId);
        //        reservation.AccessCode = Guid.NewGuid().ToString();
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, new
        //        {
        //            reservation.Id,
        //            reservation.TableId,
        //            reservation.StartTime,
        //            reservation.EndTime, 
        //            reservation.NumberOfGuests,
        //            reservation.AccessCode
        //        });
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        // -------------------- CUSTOMER RESERVATIONS --------------------
        [HttpGet("customerReservations")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReservationByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "Email parameter is required." });

            var reservations = await _reservationService.GetAllReservationsForCustomerAsync(email);

            if (reservations == null || !reservations.Any())
                return NotFound(new { message = "No reservations found for the provided email." });

            return Ok(reservations);
        }

        // -------------------- UPDATE RESERVATION --------------------
        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateReservation(ReservationDTO resDTO, int id)
        {
            var updated = await _reservationService.UpdateReservationAsync(id, resDTO);

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

        // -------------------- CREATE CUSTOMER --------------------
        [HttpPost("createCustomer")]
        [AllowAnonymous]
        public async Task<ActionResult> CreateCustomer(CustomerDTO custDTO)
        {
            var newCustId = await _reservationService.CreateCustomerAsync(custDTO);
            return CreatedAtAction(nameof(GetReservationById), new { id = newCustId }, custDTO);
        }

        // -------------------- TABLE BY ID --------------------
        [HttpGet("table/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTableById(int id)
        {
            var table = await _reservationService.GetTableByIdAsync(id);
            if (table == null)
                return NotFound(new { message = "Table not found." });

            return Ok(new
            {
                table.TableId,
                table.Capacity
            });
        }

    }
}
