using CelestialDrip.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CelestialDrip.Interfaces
{
    public interface IMachineService
    {
        Task<List<Machine>> GetMachinesAsync(int page, int pageSize, string? status);

        Task<Machine> GetMachineAsyncById(int id);

        Task<Machine> CreateMachineAsync(Machine machine);

        Task<bool> UpdateMachineStatusAsync(int id, string newStatus, byte[] rowVersion);

    }
}
