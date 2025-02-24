using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CelestialDrip.Models
{
    public class MaintenanceRecord
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string PerformedBy { get; set; }

        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("MachineId")]
        public int MachineId { get; set; }

    }
}