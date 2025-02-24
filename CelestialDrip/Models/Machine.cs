using System.ComponentModel.DataAnnotations;

namespace CelestialDrip.Models
{
    public class Machine
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public byte[] RowVersion { get; set; }
    }
}