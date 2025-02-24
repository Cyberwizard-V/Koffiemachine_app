using CelestialDrip.Models;
using CelestialDrip.Data;
using CelestialDrip.Models;

namespace CelestialDrip.Interfaces
{
    public interface IMaintenanceService
    {
        Task<List<MaintenanceRecord>> GetMaintenanceRecordsAsync(int machineId, int page, int pageSize, string? performedBy);
        Task<MaintenanceRecord> LogMaintenanceAsync(int machineId, MaintenanceRecord record);
    }
}
