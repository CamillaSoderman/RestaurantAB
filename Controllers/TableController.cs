using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAB.DTOs.TableDTOs;
using RestaurantAB.Services.Interfaces;

namespace RestaurantAB.Controllers
{
    // TABLE CONTROLLER - ADMIN ONLY

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TablesController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TablesController(ITableService tableService)
        {
            _tableService = tableService;
        }

        //  GET ALL TABLES
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tables = await _tableService.GetAllTablesAsync();
            return Ok(tables);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var table = await _tableService.GetTableByIdAsync(id);
            if (table == null) return NotFound();
            return Ok(table);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTableDTO dto)
        {
            if (dto.Capacity <= 0)
                return BadRequest("Invalid capacity");

            var id = await _tableService.CreateTableAsync(dto);

            return Ok(new { id });
        }


        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTableDTO dto)
        {
            var updated = await _tableService.UpdateTableAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _tableService.DeleteTableAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }


}
