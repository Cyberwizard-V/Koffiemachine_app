using CelestialDrip.Data;
using CelestialDrip.Models;
using CelestialDrip.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CelestialDrip.Services
{
    public class MachineService : IMachineService
    {
        private readonly CoffeeMachineContext _context;

        public MachineService(CoffeeMachineContext context)
        {
            _context = context;
        }

        public async Task<List<Machine>> GetMachinesAsync(int page, int pageSize, string? status)
        {
            var query = _context.Machines.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x => x.Status == status);
            }

            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Machine> GetMachineAsyncById(int id)
        {
            var Machine = _context.Machines.FindAsync(id);

            if (Machine == null)
                return null;

            return await _context.Machines.FindAsync(id);
        }

        public async Task<Machine> CreateMachineAsync(Machine machine)
        {
            machine.RowVersion = new byte[8];
            new Random().NextBytes(machine.RowVersion);

            _context.Machines.Add(machine);
            await _context.SaveChangesAsync();

            return machine;
        }

        public async Task<bool> UpdateMachineStatusAsync(int id, string newStatus, byte[] rowVersion)
        {
            var machine = await _context.Machines.FindAsync(id);
            if (machine == null) return false;

            if (!machine.RowVersion.SequenceEqual(rowVersion))
            {
                return false;
            }

            machine.Status = newStatus;

            try
            {
                machine.RowVersion = BitConverter.GetBytes(DateTime.UtcNow.Ticks);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
    }
}
