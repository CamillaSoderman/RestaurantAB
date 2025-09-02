using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAB.Data;
using RestaurantAB.DTOs;
using RestaurantAB.Models;

namespace RestaurantAB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly RestaurantABDbContext _context;

        public ReservationsController(RestaurantABDbContext context)
        {
            _context = context;
        }

        // GET: api/Reservations/available-tables?startTime=2023-10-10T19:00:00&NumberOfGuests=4
        [HttpGet("available)")]
        [AllowAnonymous] // Allow anonymous access to this endpoint
        public async Task<IActionResult> GetAvailableTables(DateTime startTime, int NumberOfGuests)
        {
            var tables = await _context.Tables
                .Where(t => t.Capacity >= NumberOfGuests)
                .ToListAsync();
            var availableTables = new List<Table>();

            foreach (var table in tables)
            {
                bool occupied = await _context.Reservations
                    .AnyAsync(r => r.TableId == table.Id &&
                   startTime >= r.StartTime.AddHours(-2) &&
                   startTime <= r.StartTime.AddHours(2));

                if (!occupied)
                {
                    availableTables.Add(table);
                }
            }

            return Ok(availableTables);
        }

        [HttpPost]
        [AllowAnonymous] // Allow anonymous access to this endpoint
        public async Task<ActionResult<ReservationDTO>> CreateReservation([FromBody] ReservationRequestDTO request)
        {
            // Check if the requested table is available
            var table = await _context.Tables.FindAsync(request.TableId); // check if it gets table id
            if (table == null)
            {
                return BadRequest(new { message = "Table not found." });
            }

            // Check for overlapping reservations
            var isAvailable = await _context.Reservations
                .AnyAsync(r => r.TableId == request.TableId &&
               r.StartTime < request.StartTime.AddHours(2) &&
               r.StartTime.AddHours(2) > request.StartTime);


            if (!isAvailable)
            {
                return BadRequest(new { message = "The table is already booked for the selected time." });
            }

            var customer = new Customer
            {
                CustomerName = request.CustomerName,
                CustomerPhone = request.CustomerPhone,
                CustomerEmail = request.CustomerEmail
            };

            var reservation = new Reservation
            {
                TableId = request.TableId,
                Customer = customer,
                StartTime = request.StartTime,
                NumberOfGuests = request.Guests
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        }

        // GET: api/Reservations/5 as Admin
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Reservation>> GetReservationById(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Table)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        // DELETE: api/Reservations/5 as Admin
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
