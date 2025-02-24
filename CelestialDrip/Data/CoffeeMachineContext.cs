using CelestialDrip.Models;
using Microsoft.EntityFrameworkCore;

namespace CelestialDrip.Data
{
    public class CoffeeMachineContext : DbContext
    {
        public CoffeeMachineContext(DbContextOptions<CoffeeMachineContext> options) : base(options) { }

        public DbSet<Machine> Machines { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Machine>()
                .Property(m => m.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            modelBuilder.Entity<Machine>().HasData(
                new Machine { Id = 1, Name = "Koffiemachine A", Location = "Kantine", Status = "Actief", RowVersion = BitConverter.GetBytes(DateTime.UtcNow.Ticks) },
                new Machine { Id = 2, Name = "Koffiemachine B", Location = "Vergaderzaal", Status = "Defect", RowVersion = BitConverter.GetBytes(DateTime.UtcNow.Ticks) },
                new Machine { Id = 3, Name = "Koffiemachine C", Location = "Receptie", Status = "Actief", RowVersion = BitConverter.GetBytes(DateTime.UtcNow.Ticks) },
                new Machine { Id = 4, Name = "Koffiemachine D", Location = "Lobby", Status = "Actief", RowVersion = BitConverter.GetBytes(DateTime.UtcNow.Ticks) }
            );

            modelBuilder.Entity<MaintenanceRecord>().HasData(
                new MaintenanceRecord
                {
                    Id = 1,
                    MachineId = 1,
                    Description = "Schoonmaken",
                    PerformedBy = "Apres",
                    PerformedAt = DateTime.UtcNow.AddDays(-10)
                },
                new MaintenanceRecord
                {
                    Id = 2,
                    MachineId = 2,
                    Description = "Fix water lekage",
                    PerformedBy = "Apollo",
                    PerformedAt = DateTime.UtcNow.AddDays(-5)
                },
                new MaintenanceRecord
                {
                    Id = 3,
                    MachineId = 3,
                    Description = "Filter vervanging",
                    PerformedBy = "Zeus",
                    PerformedAt = DateTime.UtcNow.AddDays(-3)
                },
                new MaintenanceRecord
                {
                    Id = 4,
                    MachineId = 4,
                    Description = "Kalibreren",
                    PerformedBy = "Artemis",
                    PerformedAt = DateTime.UtcNow.AddDays(-1)
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}