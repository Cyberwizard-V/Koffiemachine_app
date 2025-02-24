using CelestialDrip.Interfaces;
using CelestialDrip.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CoffeeMachineService.Controllers
{
    [ApiController]
    [Route("api/machines/{machineId}/maintenance")]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;

        public MaintenanceController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        [HttpPost]
        public async Task<IActionResult> LogMaintenance(int machineId, [FromBody] MaintenanceRecord record)
        {
            if (record == null)
            {
                Log.Warning("Attempt to log maintenance with null record data for machine {MachineId}.", machineId);
                return BadRequest("Maintenance record data is required.");
            }

            try
            {
                var createdRecord = await _maintenanceService.LogMaintenanceAsync(machineId, record);
                Log.Information("Successfully logged maintenance record for machine {MachineId} with record ID {RecordId}.", machineId, createdRecord.Id);

                return CreatedAtAction(nameof(GetMaintenanceRecords), new { machineId = machineId }, createdRecord);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while logging maintenance for machine {MachineId}.", machineId);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMaintenanceRecords(int machineId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? performedBy = null)
        {
            try
            {
                var records = await _maintenanceService.GetMaintenanceRecordsAsync(machineId, page, pageSize, performedBy);
                Log.Information("Successfully retrieved {RecordCount} maintenance records for machine {MachineId} from page {Page} with filter 'PerformedBy: {PerformedByFilter}'.",
                    records.Count, machineId, page, performedBy);

                return Ok(records);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving maintenance records for machine {MachineId}.", machineId);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}