using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAB.Data;
using RestaurantAB.DTOs;
using RestaurantAB.DTOs.ReservationDTOs;
using RestaurantAB.Models;
using RestaurantAB.Services.IServices;

namespace RestaurantAB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly RestaurantABDbContext _context;

        public ReservationsController(IReservationService reservationService,RestaurantABDbContext context)
        {
            _reservationService = reservationService;
            _context = context;
        }

        // GET: api/Reservations/available-tables?startTime=2023-10-10T19:00:00&NumberOfGuests=4
        [HttpGet("available")]
        [AllowAnonymous] // Allow anonymous access to this endpoint
        public async Task<ActionResult<List<ReservationDTO>>> GetAvailableTables(DateTime startTime, int NumberOfGuests)
        {
            var availableTables = await _reservationService.GetAllAvailableTablesAsync(startTime, NumberOfGuests);
            
            return Ok(availableTables);
        }

        [HttpGet("allReservations")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ReservationAdminDTO>>> GetAllReservations()

        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            if (reservations == null || !reservations.Any())
                return NotFound(new { message = "No reservations found." });


            return Ok(reservations);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReservationAdminDTO>> GetReservationById(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound(new { message = "Reservation not found." });
            }
            return Ok(reservation);
        }

        [HttpPost]
        [AllowAnonymous] // Allow anonymous access to this endpoint
        public async Task<ActionResult<ReservationDTO>> CreateReservation( ReservationRequestDTO request)
        {
            try
            {
                var reservationId = await _reservationService.CreateReservationAsync(request);
                return CreatedAtAction(nameof(GetReservationById), new { id = reservationId }, new {reservationId});
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            //var reservationId = await _reservationService.CreateReservationAsync(request);

            //return CreatedAtAction(nameof(GetReservationById), new { id = reservationId });

        }
        [HttpGet("customerReservations/")]
        public async Task<IActionResult> GetReservationByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email parameter is required." });
            }
            var reservations = await _reservationService.GetAllReservationsForCustomerAsync(email);
            
            if (reservations == null || !reservations.Any())
            {
                return NotFound(new { message = "No reservations found for the provided email." });
            }

            return Ok(reservations);
        }

        [HttpPut("update{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateReservation(ReservationDTO resDTO, int id)
        {
            var updated = await _reservationService.UpdateReservationAsync(id, resDTO);

            if (!updated)
            {
                return NotFound(new { message = "Reservation not found." });
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var deleted = await _reservationService.DeleteReservationAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Reservation not found." });
            }
            return NoContent();
        }

        [HttpPost("createCustomer")]
        [AllowAnonymous]
        public async Task<ActionResult> CreateCustomer(CustomerDTO custDTO)
        {
            var newCustId = await _reservationService.CreateCustomerAsync(custDTO);
            return CreatedAtAction(nameof(GetReservationById), new { id = newCustId }, custDTO);
        }

        //    // Previous implementation without service layer

        //    // GET: api/Reservations/available-tables?startTime=2023-10-10T19:00:00&NumberOfGuests=4
        //    [HttpGet("available)")]
        //    [AllowAnonymous] // Allow anonymous access to this endpoint
        //    public async Task<IActionResult> GetAvailableTables(DateTime startTime, int NumberOfGuests)
        //    {
        //        var tables = await _context.Tables
        //            .Where(t => t.Capacity >= NumberOfGuests)
        //            .ToListAsync();
        //        var availableTables = new List<Table>();

        //        foreach (var table in tables)
        //        {
        //            bool occupied = await _context.Reservations
        //                .AnyAsync(r => r.TableId == table.Id &&
        //               startTime >= r.StartTime.AddHours(-2) &&
        //               startTime <= r.StartTime.AddHours(2));

        //            if (!occupied)
        //            {
        //                availableTables.Add(table);
        //            }
        //        }

        //        return Ok(availableTables);
        //    }

        //    [HttpPost]
        //    [AllowAnonymous] // Allow anonymous access to this endpoint
        //    public async Task<ActionResult<ReservationDTO>> CreateReservation([FromBody] ReservationRequestDTO request)
        //    {
        //        // Check if the requested table is available
        //        var table = await _context.Tables.FindAsync(request.TableId); // check if it gets table id
        //        if (table == null)
        //        {
        //            return BadRequest(new { message = "Table not found." });
        //        }

        //        // Check for overlapping reservations
        //        var isAvailable = await _context.Reservations
        //            .AnyAsync(r => r.TableId == request.TableId &&
        //           r.StartTime < request.StartTime.AddHours(2) &&
        //           r.StartTime.AddHours(2) > request.StartTime);


        //        if (!isAvailable)
        //        {
        //            return BadRequest(new { message = "The table is already booked for the selected time." });
        //        }

        //        var customer = new Customer
        //        {
        //            CustomerName = request.CustomerName,
        //            CustomerPhone = request.CustomerPhone,
        //            CustomerEmail = request.CustomerEmail
        //        };
        //        _context.Customers.Add(customer);

        //        var reservation = new Reservation
        //        {
        //            TableId = request.TableId,
        //            Customer = customer,
        //            StartTime = request.StartTime,
        //            NumberOfGuests = request.Guests
        //        };

        //        _context.Reservations.Add(reservation);

        //        var dto = new ReservationDTO
        //        {
        //            Id = reservation.Id,
        //            TableId = reservation.TableId,
        //            StartTime = reservation.StartTime,
        //            NumberOfGuests = reservation.NumberOfGuests
        //        };



        //        return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        //    }

        //    // GET: api/Reservations/5 as Admin
        //    [HttpGet("{id:int}")]
        //    [Authorize(Roles = "Admin")]
        //    public async Task<ActionResult<ReservationDTO>> GetReservationById(int id)
        //    {
        //        var reservation = await _context.Reservations
        //            .Include(r => r.Customer)
        //            .FirstOrDefaultAsync(r => r.Id == id);

        //        if (reservation == null)
        //        {
        //            return NotFound();
        //        }

        //        return Ok(reservation);
        //    }

        //    // DELETE: api/Reservations/5 as Admin
        //    [HttpDelete("{id:int}")]
        //    [Authorize(Roles = "Admin")]
        //    public async Task<IActionResult> DeleteReservation(int id)
        //    {
        //        var reservation = await _context.Reservations.FindAsync(id);
        //        if (reservation == null)
        //        {
        //            return NotFound();
        //        }
        //        _context.Reservations.Remove(reservation);
        //        await _context.SaveChangesAsync();
        //        return NoContent();
        //    }

    }
}
