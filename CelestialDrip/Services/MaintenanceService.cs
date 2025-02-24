using CelestialDrip.Data;
using CelestialDrip.Models;
using CelestialDrip.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace CelestialDrip.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly CoffeeMachineContext _context;

        public MaintenanceService(CoffeeMachineContext context)
        {
            _context = context;
        }

        public async Task<List<MaintenanceRecord>> GetMaintenanceRecordsAsync(int machineId, int page, int pageSize, string? performedBy)
        {
            var query = _context.MaintenanceRecords.AsQueryable();

            if (machineId > 0)
            {
                query = query.Where(x => x.MachineId == machineId);
            }

            if (!string.IsNullOrEmpty(performedBy))
            {
                query = query.Where(x => x.PerformedBy.Contains(performedBy));
            }

            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<MaintenanceRecord> LogMaintenanceAsync(int machineId, MaintenanceRecord record)
        {
            record.MachineId = machineId;
            _context.MaintenanceRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }
    }
} 