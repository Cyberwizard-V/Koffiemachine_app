using CelestialDrip.Interfaces;
using CelestialDrip.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Machine = CelestialDrip.Models.Machine;

namespace CelestialDrip.Controllers
{
    [ApiController]
    [Route("api/machines")]
    public class MachinesController : Controller
    {
        private readonly IMachineService _machineService;

        public MachinesController(IMachineService machineService)
        {
            _machineService = machineService;
        }

        //register machine
        [HttpPost]
        public async Task<IActionResult> CreateMachine([FromBody] Machine machine)
        {
            try
            {
                var createdMachine = await _machineService.CreateMachineAsync(machine);

                Log.Information("Machine creation => {@createdMachine}", createdMachine); // logggg
                return CreatedAtAction(nameof(GetMachineById), new { id = createdMachine.Id }, createdMachine); // obj dat 201 terug geeft
            } 
            catch (Exception ex)
            {
                Log.Error(ex, "Error occured while creating machine");
                return StatusCode(500, "Internal server error"); // return 500 voor ISE
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMachines([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? status = null)
        {
            try
            {
                var machines = await _machineService.GetMachinesAsync(page, pageSize, status);
                
                Log.Information("Successfully retrieved {MachineCount} machines from page {Page}", machines.Count, page);

                return Ok(machines);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occured while retrieving machines");
                return StatusCode(500, "Internal server error"); // return 500 voor ISE
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMachineById(int id)
        {
            try
            {
                var machine = await _machineService.GetMachineAsyncById(id);

                if (machine == null)
                {
                    Log.Warning("Machine with ID {MachineId} not found", id);
                    return NotFound();
                }

                Log.Information("Successfully retrieved machine with Id {MachineId}", id);
                return Ok(machine);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving machine with Id {MachineId}", id);
                return StatusCode(500, "Internal server error.");
            }
        }
        //Update status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateMachineStatus(int id, [FromBody] MachineStatusUpdateRequest request)
        {
            if (request == null)
            {
                Log.Warning("Attempt to update machine status with null request data");
                return BadRequest("Request data is required");
            }

            try
            {
                var success = await _machineService.UpdateMachineStatusAsync(id, request.Status, request.RowVersion);

                if (!success)
                {
                    Log.Warning("Concurrency conflict when updating status for machine with Id {MachineId}", id);
                    return Conflict("Concurrency conflict: Data changed."); // 409 voor conflicts
                }

                Log.Information("Successfully updated the status of machine with Id {MachineId} to '{Status}'", id, request.Status);
                return Ok(success); //200, kan ook 204 zijn.
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating status for machine with Id {MachineId}", id);
                return StatusCode(500, "Internal server error."); // return 500 voor ISE
            }
        }
    }
}
